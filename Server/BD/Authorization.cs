using Butterfly;

namespace server.BD
{
    public sealed class AuthorizationShell : Controller.LocalField<Setting.BD>
    {
        public const string NAME = "Authroziation";

        public struct BUS
        {
            public struct Message
            {
                public const string VERIFICATION = NAME + ":Verification";
            }
        }

        void Construction()
        {
            extract_values<client.connect.Data>
            (BUS.Message.VERIFICATION, (clients) =>
            {
                // ... 
            });
        }

        void Start() => obj<_object>(NAME);

        private sealed class _object : Controller.Board
        {
            void Construction()
            {
                safe_listen_message<client.connect.Data>(BUS.Message.VERIFICATION,
                    Header.Events.BD_AUTHORIZATION, AuthorizationShell.NAME)
                        .output_to((client, confirm) =>
                        {
                            client.IsSuccessVerification();

                            confirm();
                        });
            }
        }
    }
}