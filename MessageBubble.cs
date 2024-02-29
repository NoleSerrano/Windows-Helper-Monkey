using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

public class MessageBubble : Form
{
    private Label messageLabel;
    private System.Windows.Forms.Timer topMostTimer;

    public MessageBubble(string message)
    {
        InitializeComponents();
        SetMessage(message);
        MakeTopMost();
    }

    private void InitializeComponents()
    {
        this.messageLabel = new Label
        {
            AutoSize = true,
            ForeColor = Color.Black,
            Location = new Point(10, 10),
            Size = new Size(180, 80),
            Text = string.Empty
        };

        this.Controls.Add(this.messageLabel);

        this.FormBorderStyle = FormBorderStyle.None;
        this.BackColor = Color.LightYellow;
        this.Size = new Size(200, 100);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.TopMost = true;
    }

    private void SetMessage(string message)
    {
        this.messageLabel.Text = message;
    }

    private void MakeTopMost()
    {
        topMostTimer = new System.Windows.Forms.Timer();
        topMostTimer.Interval = 500; // Set the interval to a suitable value (e.g., 500 ms).
        topMostTimer.Tick += (sender, e) =>
        {
            this.TopMost = true;
            this.Activate();
        };
        topMostTimer.Start();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);
        if (topMostTimer != null)
        {
            topMostTimer.Stop();
            topMostTimer.Dispose();
        }
    }

    public static void ShowMessageBubble(string message)
    {
        Thread thread = new Thread(() =>
        {
            Application.Run(new MessageBubble(message));
        })
        {
            IsBackground = true
        };
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
    }
}
