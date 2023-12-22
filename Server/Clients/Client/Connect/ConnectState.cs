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

        private const string END_CONNECTION_CLIENT = "End connection client";

        private string CurrentState = RECEIVE_LOGIN_AND_PASSWORD;

        private const string CHANGING_INFO
            = @"CurrentState:{0} \n ChangingTheState{1} - access.";
        private const string CHANGING_ERROR
            = @"CurrentState:{0} \n ChangingTheState{1} - error.";
        private const string DESTROY_INFO
            = @"CurrentState:{0} \n ChangingTheState{1} - destroy.";

        private bool _isDestroy = false;

        public bool IsDestroy()
        { lock (this) return _isDestroy; }

        public void Destroy()
        { lock (this) _isDestroy = true; }

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
            lock (this)
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
        }

        public bool HasBeginSubscribeToReceiveTcpConnection()
            => CurrentState == BEGIN_SUBSCRIBE_TO_RECEIVE_TCP_CONNECTION;

        public bool SetSendingAccessVerificationAndRequestTcpConnection(out string info)
        {
            lock (this)
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

        public bool SetEndConnectionClient(out string info)
        {
            lock (this)
            {
                if (CurrentState == SENDING_ACCESS_VERIFICATION_AND_REQUEST_TCP_CONNECTION)
                {
                    if (_isDestroy)
                    {
                        info = String.Format
                            (DESTROY_INFO, SENDING_ACCESS_VERIFICATION_AND_REQUEST_TCP_CONNECTION, 
                                END_CONNECTION_CLIENT);

                        return false;
                    }

                    CurrentState = END_CONNECTION_CLIENT;

                    info = String.Format
                        (DESTROY_INFO, SENDING_ACCESS_VERIFICATION_AND_REQUEST_TCP_CONNECTION, 
                            END_CONNECTION_CLIENT);

                    return true;
                }
                else
                {
                    info = String.Format
                        (DESTROY_INFO, SENDING_ACCESS_VERIFICATION_AND_REQUEST_TCP_CONNECTION, 
                            END_CONNECTION_CLIENT);

                    return false;
                }
            }
        }
    }
}