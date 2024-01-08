using Butterfly;

namespace server.client
{
    public class Connected : gameSession.Controller
    {
        void Construction()
        {
            BDData.Index = Field.BDIndex;

            input_to(ref I_sendMessageToClient, Header.Events.TCP_SEND, Field.Tcp.Send);
            input_to(ref I_sendMessagesToClient, Header.Events.TCP_SEND, Field.Tcp.Send);

            send_message(ref I_clientLogger, Logger.Type.CLIENT);

            input_to(ref I_process, Header.Events.SYSTEM, Process);

            input_to(ref BDData.I_BDReceiveData, Header.Events.SYSTEM, BDReceiveData);

            safe_send_message(ref I_DBLoadData, BD.ClientData.BUS.Message.GetData);

            Field.Tcp.Destroy = Destroy;

            input_to(ref Field.Tcp.I_output, Header.Events.WORK, ReceiveTcp);

            add_event(Header.Events.TCP_RECEIVE, Field.Tcp.Receive);

            send_echo_1_1<world.room.Setting, world.room.Controller.IReceive>
                (ref I_addToWorld, World.BUS.Echo.CREATING)
                    .output_to((roomReceive) =>
                    {
                        LoggerInfo($"Комната успешно создана.");
                    });

            send_echo_1_1<string, bool>(ref I_removeFromWorld, World.BUS.Echo.REMOVE)
                .output_to((result) =>
                {
                    if (result)
                    {
                    }
                    else
                    {
                    }
                });

            input_to(ref IRoom_creating, Header.Events.CLIENT, (roomName, positionX, positionY) =>
            {
                lock (State.Locker)
                {
                    if (State.HasInputToWorld())
                    {
                        if (State.SetCreateRoom(out string info))
                        {
                            LoggerInfo($"Room receive:{roomName} creating.");

                            I_sendMessagesToClient.To(new byte[][]
                            {
                                GetCreatingCharacterMessage(),
                                GetCreatingRoomMessage(),
                                GetNextCharacterPositionMessage(positionX, positionY),
                            });
                        }
                        else LoggerError(info);
                    }
                    else LoggerWarning("В момент получения сообщения о создании" +
                        "комнаты обьект начал процесс уничтожения.");
                }
            });

            input_to(ref IRoom_characterMove, Header.Events.CLIENT, (direction, positionX, positionY,
                dateTime, trevalTimeMill) =>
                {
                    lock (State.Locker)
                    {
                        if (State.HasCreateRoom())
                        {
                            LoggerInfo($"Character move:Direction[{direction}], PositionX[{positionX}], " + 
                                $"PositionY[{positionY}], DateTime[{dateTime}], TrevalTimeMill[{trevalTimeMill}].");

                            I_sendMessageToClient.To(GetMoveCharacterPositionMessage
                                (direction, positionX, positionY, dateTime, trevalTimeMill));
                        }
                        else LoggerWarning("Пришло сообщение из комнаты в момент когда состояния клента: " + 
                            $"{State.CurrentState}.");
                    }
                });
        }


        void Start()
        {
            LoggerInfo("Start");

            Field.Tcp.Start();

            I_process.To();
        }

        public interface IClient
        {
            public string GetNickname();
        }
    }
}
