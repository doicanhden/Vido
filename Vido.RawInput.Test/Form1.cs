namespace Vido.RawInput.Test
{
  using System;
  using System.Windows.Forms;
  using Vido.RawInput.Events;
  using Vido.RawInput;

  public partial class Form1 : Form
  {
    private RawInput rawInput;
    public Form1()
    {
      InitializeComponent();
      rawInput = new RawInput(Handle);
      rawInput.AddMessageFilter();
      rawInput.Keyboard.DevicesChanged += Keyboards_DevicesChanged;
      rawInput.Keyboard.EnumerateDevices();
    }

    private void Keyboards_DevicesChanged(object s, DevicesChangedEventArgs e)
    {
      listBox1.Items.Add("Devices Changed");
      if (e.OldDevices != null)
      {
        foreach (var keyboard in e.OldDevices)
        {
          keyboard.KeyDown -= keyboard_KeyDown;
          keyboard.KeyUp -= keyboard_KeyUp;
        }
      }
      if (e.NewDevices != null)
      {
        foreach (var keyboard in e.NewDevices)
        {
          keyboard.KeyDown += keyboard_KeyDown;
          keyboard.KeyUp += keyboard_KeyUp;
        }
      }
    }

    void keyboard_KeyUp(object sender, EventArgs e)
    {
      var args = e as Vido.RawInput.Events.KeyEventArgs;
      var s = sender as IKeyboard;

      listBox1.Items.Add(string.Format("Keyboard: {0}, Key up: {1}", s.Description, args.KeyValue));
      listBox1.SelectedItem = listBox1.Items.Count - 1;
    }

    void keyboard_KeyDown(object sender, EventArgs e)
    {
      var args = e as Vido.RawInput.Events.KeyEventArgs;
      var s = sender as IKeyboard;

      listBox1.Items.Add(string.Format("Keyboard: {0}, Key down: {1}", s.Description, args.KeyValue));
      listBox1.SelectedItem = listBox1.Items.Count - 1;
    }
  }
}
