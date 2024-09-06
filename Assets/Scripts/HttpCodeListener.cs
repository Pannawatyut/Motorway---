using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

public class HttpCodeListener
{
    private HttpListener listener;
    private Thread listenerThread;
    private Action<string> onCodeFetched;

    private const string responseHtml = "http://localhost:57847/"; // TODO: Change this to a successful html response

    public HttpCodeListener()
    {
        listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:57847/"); // Updated to the new redirect URI
        listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
    }

    public void StartListening(Action<string> onCodeReceived)
    {
        listener.Start();
        listener.BeginGetContext(ar =>
        {
            var context = listener.EndGetContext(ar);
            var request = context.Request;
            var response = context.Response;

            if (request.QueryString.HasKeys())
            {
                string code = request.QueryString["code"];
                onCodeReceived?.Invoke(code);

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseHtml);
                response.ContentLength64 = buffer.Length;
                var output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }

            listener.Stop();
        }, null);
    }

    public void StopListening()
    {
        if (listener.IsListening)
        {
            listener.Stop();
        }
    }

    private void ListeningThread()
    {
        while (listener.IsListening)
        {
            var result = listener.BeginGetContext(ListenerCallback, listener);
            result.AsyncWaitHandle.WaitOne();
        }
    }

    private void ListenerCallback(IAsyncResult result)
    {
        var context = listener.EndGetContext(result);

        if (!context.Request.QueryString.AllKeys.Contains("code")) return;
        UnityMainThreadDispatcher.Instance().Enqueue(() => onCodeFetched?.Invoke(context.Request.QueryString.Get("code")));

        var buffer = Encoding.UTF8.GetBytes(responseHtml);

        context.Response.ContentLength64 = buffer.Length;
        var st = context.Response.OutputStream;
        st.Write(buffer, 0, buffer.Length);

        context.Response.Close();
    }
}