using System.Data;
using Butterfly;

namespace server.client.gameSession
{
    public abstract class Main : Butterfly.Controller.Board.LocalField<client.ConnectedInformation>
    {
        protected const string NAME = "GameSession";

        protected BDData BDData = new();
        protected IInput I_process;
        protected readonly State State = new();
        protected IInput<int, string> I_clientLogger;

        //---------------------BD---------------------------
        /// <summary>
        /// Загружаем данные клиента.
        /// </summary>
        protected IInput<BDData> I_DBLoadData;
        //--------------------------------------------------


        //-----------------[Clinet->World]------------------
        protected IInput<world.room.Setting> I_addToWorld;
        protected IInput<string> I_removeFromWorld;
        //--------------------------------------------------
        protected IInput<byte[]> I_sendMessageToClient;
        protected IInput<byte[][]> I_sendMessagesToClient;

        protected Data Data = new();

        protected byte[] GetMoveCharacterPositionMessage(int positionX, int positionY)
        {
            return new byte[NetWork.Server.CharacterMove.LENGTH]
            {
                NetWork.Server.CharacterMove.LENGTH >> 8,
                NetWork.Server.CharacterMove.LENGTH,

                NetWork.Server.CharacterMove.TYPE >> 8,
                NetWork.Server.CharacterMove.TYPE,

                (byte)(positionX >> 24), // Позиция в которую нужно двигаться.
                (byte)(positionX >> 16), // Позиция в которую нужно двигаться.
                (byte)(positionX >> 8), // Позиция в которую нужно двигаться.
                (byte)positionX, // Позиция в которую нужно двигаться.

                (byte)(positionY >> 24), // Позиция в которую нужно двигаться.
                (byte)(positionY >> 16), // Позиция в которую нужно двигаться.
                (byte)(positionY >> 8), // Позиция в которую нужно двигаться.
                (byte)positionY, // Позиция в которую нужно двигаться.
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

                (byte)(positionX >> 24), // PositionX
                (byte)(positionX >> 16), // PositionX
                (byte)(positionX >> 8), // PositionX
                (byte)positionX, // PositionX

                (byte)(positionY >> 24), // PositionY
                (byte)(positionY >> 16), // PositionY
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

        protected byte[] GetCharacterStartMoveMessage()
        {
            return new byte[NetWork.Server.CharacterRun.LENGTH]
            {
                NetWork.Server.CharacterRun.LENGTH >> 8,
                NetWork.Server.CharacterRun.LENGTH,

                NetWork.Server.CharacterRun.TYPE >> 8,
                NetWork.Server.CharacterRun.TYPE,
            };
        }

        protected byte[] GetCharacterStopMessage()
        {
            return new byte[NetWork.Server.CharacterStop.LENGTH]
            {
                NetWork.Server.CharacterStop.LENGTH >> 8,
                NetWork.Server.CharacterStop.LENGTH,

                NetWork.Server.CharacterStop.TYPE >> 8,
                NetWork.Server.CharacterStop.TYPE,
            };
        }

        protected byte[] GetCharacterDirectionMessage(int direction)
        {
            if (direction == 0)
            {
                return new byte[NetWork.Server.CharacterDirectionRigth.LENGTH]
                {
                    NetWork.Server.CharacterDirectionRigth.LENGTH >> 8,
                    NetWork.Server.CharacterDirectionRigth.LENGTH,

                    NetWork.Server.CharacterDirectionRigth.TYPE >> 8,
                    NetWork.Server.CharacterDirectionRigth.TYPE,
                };

            }
            else if (direction == 1)
            {
                return new byte[NetWork.Server.CharacterDirectionLeft.LENGTH]
                {
                    NetWork.Server.CharacterDirectionLeft.LENGTH >> 8,
                    NetWork.Server.CharacterDirectionLeft.LENGTH,

                    NetWork.Server.CharacterDirectionLeft.TYPE >> 8,
                    NetWork.Server.CharacterDirectionLeft.TYPE,
                };
            }
            else throw new Exception();
        }

        protected byte[] GetCharacterMoveSpeed()
        {
            return new byte[NetWork.Server.CharacterMoveSpeed.LENGTH]
            {
                NetWork.Server.CharacterMoveSpeed.LENGTH >> 8,
                NetWork.Server.CharacterMoveSpeed.LENGTH,

                NetWork.Server.CharacterMoveSpeed.TYPE >> 8,
                NetWork.Server.CharacterMoveSpeed.TYPE,

                (byte)(Data.SpeedMove >> 8),
                (byte)(Data.SpeedMove)
            };
        }
    }
}