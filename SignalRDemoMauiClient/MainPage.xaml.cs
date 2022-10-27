using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRDemoMauiClient
{
    public partial class MainPage : ContentPage
    {
        private HubConnection _hubConnection;

        public MainPage()
        {
            InitializeComponent();

            try
            {
                messages.Text = string.Empty;

                var ip = "localhost";
                _hubConnection = new HubConnectionBuilder().WithUrl($"https://{ip}:7177/chatHub").Build();

                _hubConnection.On<string, string>("ReceiveMessage", MessageReceived);
            }
            catch (Exception e)
            {
                // Some error handling.
            }
        }

        protected async override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            await _hubConnection.StartAsync();
        }

        private async void sendBtn_Clicked(object sender, EventArgs e)
        {
            await _hubConnection.SendAsync("SendMessage", username.Text, message.Text);

            username.Text = string.Empty;
            message.Text = string.Empty;
        }

        private void MessageReceived(string username, string message)
        {
            messages.Text += $"{username} said {message}{Environment.NewLine}";
        }
    }
}