using System.Windows.Controls;
using SocketIOClient;

namespace Memory.ui.pages
{
    /// <summary>
    ///     Interaction logic for PreGame.xaml
    /// </summary>
    public partial class OnlinePage : Page
    {
        private readonly SocketIO client;

        public OnlinePage()
        {
            client = new SocketIO("http://localhost:11000/");
            client.ConnectAsync();
        }
    }
}