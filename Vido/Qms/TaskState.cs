﻿// Copyright (C) 2014 Vido's R&D.  All rights reserved.

namespace Vido.Qms
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;

  internal class TaskState
  {
    private readonly Task task;

    public IGate Gate { get; private set; }
    public ConsumerQueue<EntryArgs> Entries { get; private set; }

    public EventWaitHandle EntryAllow { get; private set; }
    public EventWaitHandle EntryBlock { get; private set; }
    public EventWaitHandle TaskStop { get; private set; }

    public TaskState(IGate gate, Action<TaskState> workerThread)
    {
      this.Gate = gate;
      this.Entries = new ConsumerQueue<EntryArgs>();
      this.EntryAllow = gate.Allow;
      this.EntryBlock = gate.Block;
      this.TaskStop = new ManualResetEvent(false);

      this.task = new Task((x) => workerThread(x as TaskState), this);
      this.task.Start();
    }

    public void Close()
    {
      TaskStop.Set();
      task.Wait();
    }
  }
}