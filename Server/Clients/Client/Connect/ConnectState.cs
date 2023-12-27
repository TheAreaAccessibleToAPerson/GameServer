namespace server.client.connect
{
    public sealed class State
    {
        private const string NONE = "None";

        private const string RECEIVE_LOGIN_AND_PASSWORD = "ReceiveLoginAndPassword";

        private const string VERIFICATION = "Verification";

        private const string ERROR_LOGIN = "Error loggin";

        private const string ERROR_PASSWORD = "Error password";

        private const string BEGIN_SUBSCRIBE_TO_RECEIVE_TCP_CONNECTION
            = "Begin subscribe to receive connection.";
        private const string END_SUBSCRIBE_TO_RECEIVE_TCP_CONNECTION
            = "Begin subscribe to receive connection.";

        private const string SENDING_ACCESS_VERIFICATION_AND_REQUEST_TCP_CONNECTION
            = "Sending access verification and request tcp connection.";

        private const string UNSUBSCRIBE_TCP_CONNECT = "Unsubscribe Tcp connect.";
        private const string END_CONNECTION_CLIENT = "End connection client";

        public string CurrentState { private set; get; } = RECEIVE_LOGIN_AND_PASSWORD;

        private const string CHANGING_INFO
            = @"CurrentState:{0} -> {1} - access.";
        private const string CHANGING_ERROR
            = @"CurrentState:{0} -> {1} - error.";
        private const string DESTROY_INFO
            = @"CurrentState:{0} -> {1} - destroy.";

        public readonly object Locker = new object();

        private bool _isDestroy = false;

        public bool IsDestroy()
        {
            lock (Locker) return _isDestroy;
        }

        public void Destroy()
        {
            lock (Locker)
            {
                _isDestroy = true;
            }
        }

        public string StepError()
            => $"CurrentState:{CurrentState}. Вы пытаетесь выполнить лишний шаг.";

        public bool HasReceiveLoginAndPassword()
            => CurrentState == RECEIVE_LOGIN_AND_PASSWORD;

        public bool SetVerification(out string info)
        {
            if (CurrentState == RECEIVE_LOGIN_AND_PASSWORD)
            {
                if (_isDestroy)
                {
                    info = String.Format
                        (DESTROY_INFO, RECEIVE_LOGIN_AND_PASSWORD, VERIFICATION);

                    return false;
                }

                CurrentState = VERIFICATION;

                info = String.Format
                    (CHANGING_INFO, RECEIVE_LOGIN_AND_PASSWORD, VERIFICATION);

                return true;
            }
            else
            {
                info = String.Format
                    (CHANGING_ERROR, RECEIVE_LOGIN_AND_PASSWORD, VERIFICATION);

                return false;
            }
        }

        public bool HasVerification()
            => CurrentState == VERIFICATION;

        public bool SetBeginSubscribeToReceiveTcpConnection(out string info)
        {
            if (CurrentState == VERIFICATION)
            {
                if (_isDestroy)
                {
                    info = String.Format
                        (DESTROY_INFO, VERIFICATION,
                            BEGIN_SUBSCRIBE_TO_RECEIVE_TCP_CONNECTION);

                    return false;
                }

                CurrentState = BEGIN_SUBSCRIBE_TO_RECEIVE_TCP_CONNECTION;

                info = String.Format
                    (CHANGING_INFO, VERIFICATION,
                        BEGIN_SUBSCRIBE_TO_RECEIVE_TCP_CONNECTION);

                return true;
            }
            else
            {
                info = String.Format
                    (CHANGING_ERROR, VERIFICATION,
                        BEGIN_SUBSCRIBE_TO_RECEIVE_TCP_CONNECTION);

                return false;
            }
        }

        public bool HasBeginSubscribeToReceiveTcpConnection()
            => CurrentState == BEGIN_SUBSCRIBE_TO_RECEIVE_TCP_CONNECTION;

        public bool SetSendingAccessVerificationAndRequestTcpConnection(out string info)
        {
            //lock (Locker)
            {
                if (CurrentState == BEGIN_SUBSCRIBE_TO_RECEIVE_TCP_CONNECTION)
                {
                    if (_isDestroy)
                    {
                        info = String.Format
                            (DESTROY_INFO, BEGIN_SUBSCRIBE_TO_RECEIVE_TCP_CONNECTION,
                                SENDING_ACCESS_VERIFICATION_AND_REQUEST_TCP_CONNECTION);

                        return false;
                    }

                    CurrentState = SENDING_ACCESS_VERIFICATION_AND_REQUEST_TCP_CONNECTION;

                    info = String.Format
                        (DESTROY_INFO, BEGIN_SUBSCRIBE_TO_RECEIVE_TCP_CONNECTION,
                            SENDING_ACCESS_VERIFICATION_AND_REQUEST_TCP_CONNECTION);

                    return true;
                }
                else
                {
                    info = String.Format
                        (DESTROY_INFO, BEGIN_SUBSCRIBE_TO_RECEIVE_TCP_CONNECTION,
                            SENDING_ACCESS_VERIFICATION_AND_REQUEST_TCP_CONNECTION);

                    return false;
                }
            }
        }

        public bool HasSendingAccessVerificationAndRequestTcpConnect()
            => CurrentState == SENDING_ACCESS_VERIFICATION_AND_REQUEST_TCP_CONNECTION;

        public bool SetUnsubscribeRequestTcpConnect(out string info)
        {
            //lock (Locker)
            {
                if (CurrentState == SENDING_ACCESS_VERIFICATION_AND_REQUEST_TCP_CONNECTION)
                {
                    if (_isDestroy)
                    {
                        info = String.Format
                            (DESTROY_INFO, SENDING_ACCESS_VERIFICATION_AND_REQUEST_TCP_CONNECTION,
                                UNSUBSCRIBE_TCP_CONNECT);

                        return false;
                    }

                    CurrentState = UNSUBSCRIBE_TCP_CONNECT;

                    info = String.Format
                        (DESTROY_INFO, SENDING_ACCESS_VERIFICATION_AND_REQUEST_TCP_CONNECTION,
                            UNSUBSCRIBE_TCP_CONNECT);

                    return true;
                }
                else
                {
                    info = String.Format
                        (DESTROY_INFO, SENDING_ACCESS_VERIFICATION_AND_REQUEST_TCP_CONNECTION,
                            UNSUBSCRIBE_TCP_CONNECT);

                    return false;
                }
            }
        }

        public bool HasUnsubscribeReqestTcpConnect()
            => CurrentState == UNSUBSCRIBE_TCP_CONNECT;

        public bool SetEndConnectionClinet(out string info)
        {
            //lock (Locker)
            {
                if (CurrentState == UNSUBSCRIBE_TCP_CONNECT)
                {
                    if (_isDestroy)
                    {
                        info = String.Format
                            (DESTROY_INFO, UNSUBSCRIBE_TCP_CONNECT,
                                END_CONNECTION_CLIENT);

                        return false;
                    }

                    CurrentState = END_CONNECTION_CLIENT;

                    info = String.Format
                        (DESTROY_INFO, UNSUBSCRIBE_TCP_CONNECT,
                            END_CONNECTION_CLIENT);

                    return true;
                }
                else
                {
                    info = String.Format
                        (DESTROY_INFO, UNSUBSCRIBE_TCP_CONNECT,
                            END_CONNECTION_CLIENT);

                    return false;
                }
            }
        }

        public bool HasEndClientConnection()
            => CurrentState == END_CONNECTION_CLIENT;
    }
}