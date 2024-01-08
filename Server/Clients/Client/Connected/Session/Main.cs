using System.Data;
using Butterfly;

namespace server.client.gameSession
{
    public abstract class Main : Butterfly.Controller.Board.LocalField<client.ConnectedInformation>
    {
        protected IInput<byte[]> I_sendMessageToClient;
        protected IInput<byte[][]> I_sendMessagesToClient;

        protected Data Data = new();

        protected void SendCreatingRoom(int positionX, int positionY)
        {
            I_sendMessageToClient.To(new byte[NetWork.Server.CreatingRoom.LENGTH]
            {
                NetWork.Server.CreatingRoom.LENGTH >> 8,
                NetWork.Server.CreatingRoom.LENGTH,

                NetWork.Server.CreatingRoom.TYPE >> 8,
                NetWork.Server.CreatingRoom.TYPE,

                0, // Имя комнаты.
                1, // Имя комнаты.
            });
        }
        protected byte[] GetMoveCharacterPositionMessage(int direction, int positionX, int positionY,
            System.DateTime dateTime, int trevalTimeMill)
        {
            return new byte[NetWork.Server.CharacterMove.LENGTH] 
            {
                NetWork.Server.CharacterMove.LENGTH >> 8,
                NetWork.Server.CharacterMove.LENGTH,

                NetWork.Server.CharacterMove.TYPE >> 8,
                NetWork.Server.CharacterMove.TYPE,

                (byte)direction, // Направление.

                (byte)(positionX >> 24), // Позиция в которую нужно двигаться.
                (byte)(positionX >> 16), // Позиция в которую нужно двигаться.
                (byte)(positionX >> 8), // Позиция в которую нужно двигаться.
                (byte)positionX, // Позиция в которую нужно двигаться.

                (byte)(positionY >> 24), // Позиция в которую нужно двигаться.
                (byte)(positionY >> 16), // Позиция в которую нужно двигаться.
                (byte)(positionY >> 8), // Позиция в которую нужно двигаться.
                (byte)positionY, // Позиция в которую нужно двигаться.

                (byte)(dateTime.Millisecond >> 8), (byte)dateTime.Millisecond,
                (byte)dateTime.Second, (byte)dateTime.Minute, (byte)dateTime.Hour, 
                (byte)dateTime.Day, (byte)dateTime.Month, (byte)dateTime.Year,

                (byte)(trevalTimeMill >> 24), 
                (byte)(trevalTimeMill >> 16), 
                (byte)(trevalTimeMill >> 8), 
                (byte)trevalTimeMill
            };
        }

        protected byte[] GetNextCharacterPositionMessage(int positionX, int positionY)
        {
            return new byte[NetWork.Server.NextCharacterPosition.LENGTH]
            {
                NetWork.Server.NextCharacterPosition.LENGTH >> 8,
                NetWork.Server.NextCharacterPosition.LENGTH,

                NetWork.Server.NextCharacterPosition.TYPE >> 8,
                NetWork.Server.NextCharacterPosition.TYPE,

                (byte)(positionX >> 8), // PositionX
                (byte)positionX, // PositionX

                (byte)(positionY >> 8), // PositionY
                (byte)(positionY), // PositionY
            };
        }

        protected byte[] GetCreatingCharacterMessage()
        {
            return new byte[NetWork.Server.CreatingCharacter.LENGTH]
            {
                NetWork.Server.CreatingCharacter.LENGTH >> 8,
                NetWork.Server.CreatingCharacter.LENGTH,

                NetWork.Server.CreatingCharacter.TYPE >> 8,
                NetWork.Server.CreatingCharacter.TYPE,

                (byte)(Data.CharacterName >> 8), // Наименование скрипта.
                (byte)(Data.CharacterName), // Наименование скрипта.
            };
        }

        protected byte[] GetCreatingRoomMessage()
        {
            return new byte[NetWork.Server.CreatingRoom.LENGTH]
            {
                NetWork.Server.CreatingRoom.LENGTH >> 8,
                NetWork.Server.CreatingRoom.LENGTH,

                NetWork.Server.CreatingRoom.TYPE >> 8,
                NetWork.Server.CreatingRoom.TYPE,

                0, // Имя комнаты.
                1, // Имя комнаты.
            };
        }
    }

}