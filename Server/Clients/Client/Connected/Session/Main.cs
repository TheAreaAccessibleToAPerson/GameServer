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
        protected IInput<byte[], byte[]> I_send2MessagesToClient;
        protected IInput<byte[], byte[], byte[]> I_send3MessagesToClient;
        protected IInput<byte[], byte[], byte[], byte[]> I_send4MessagesToClient;
        protected IInput<byte[], byte[], byte[], byte[], byte[]> I_send5MessagesToClient;

        protected Data Data = new();

        protected void SubstractMP(int value)
        {
            int currentMP = Data.CurrentMP;

            if (currentMP > 0)
            {
            }
            else 
            {
            }
        }

        protected void DefaultAttack(int speed, int damage)
        {
            LoggerInfo($"Начало атаки со скоростью{speed}");

            I_sendMessageToClient.To(GetCharacterDefaultAttack(speed, damage));
        }

        /// <summary>
        /// Вычитаем урон из щита и сдоровья.
        /// </summary>
        /// <param name="value"></param>
        protected void SubstractHS(int value)
        {
            int currentShield = Data.CurrentShield;
            int s = currentShield - value;
            if (s >= 0)
            {
                LoggerInfo($"SubstractShield:{currentShield} - {value} = {s}");

                // Если весь урон прошел по щиту.
                Data.CurrentShield = s;

                I_sendMessageToClient.To(GetCharacterCurrentShield(s));

                return;
            }
            else 
            {
                LoggerInfo($"SubstractShield:{currentShield} - {value} = {s}");

                // Если урона было больше, то выставим текущий щит в 0.
                Data.CurrentShield = 0;

                // Оставшийся урон сделаем положительным.
                value = s * -1;
            }

            int currentHP = Data.CurrentHP;
            int h = currentHP - value;
            if (h > 0)
            {
                LoggerInfo($"SubstractHP:{currentHP} - {value} = {h}");

                // Полученый урон не убил нас.
                Data.CurrentHP = h;

                I_send2MessagesToClient.To
                (
                    GetCharacterCurrentShield(s),
                    GetCharacterCurrentHP(h)
                );

                return;
            }
            else 
            {
                LoggerInfo($"SubstractHP:{currentHP} - {value} = {h} (Death).");

                // Смерть.
            }
        }

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

        protected byte[] GetCharacterMoveSpeedMessage()
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

        protected byte[] GetCreatingMobMessage(unit.Mob mob)
        {
            return new byte[NetWork.Server.CreatingMob.LENGTH]
            {
                NetWork.Server.CreatingMob.LENGTH >> 8,
                NetWork.Server.CreatingMob.LENGTH,

                (byte)(NetWork.Server.CreatingMob.TYPE >> 8),
                unchecked((byte)NetWork.Server.CreatingMob.TYPE),

                (byte)(mob.Name >> 8), (byte)mob.Name,

                (byte)(mob.ID >> 8), (byte)mob.ID,

                (byte)mob.Direction,

                (byte)(mob.PositionX >> 24), // PositionX
                (byte)(mob.PositionX >> 16), // PositionX
                (byte)(mob.PositionX >> 8), // PositionX
                (byte)mob.PositionX, // PositionX

                (byte)(mob.PositionY >> 24), // PositionY
                (byte)(mob.PositionY >> 16), // PositionY
                (byte)(mob.PositionY >> 8), // PositionY
                (byte)(mob.PositionY), // PositionY
            };
        }

        protected byte[] GetCreatingEnemyMobBar(unit.Mob mob)
        {
            return new byte[NetWork.Server.CreatingEnemyMobBar.LENGTH]
            {
                NetWork.Server.CreatingEnemyMobBar.LENGTH >> 8,
                NetWork.Server.CreatingEnemyMobBar.LENGTH,

                (byte)(NetWork.Server.CreatingEnemyMobBar.TYPE >> 8),
                unchecked((byte)NetWork.Server.CreatingEnemyMobBar.TYPE),

                (byte)(mob.BarName >> 8), (byte)(mob.BarName),

                (byte)(mob.HP >> 24), (byte)(mob.HP >> 16),
                (byte)(mob.HP >> 8), (byte)(mob.HP),

                (byte)(mob.MP >> 24), (byte)(mob.MP >> 16),
                (byte)(mob.MP >> 8), (byte)(mob.MP),

                (byte)(mob.Shield >> 24), (byte)(mob.Shield >> 16),
                (byte)(mob.Shield >> 8), (byte)(mob.Shield),

                (byte)(mob.CurrentHP >> 24), (byte)(mob.CurrentHP >> 16),
                (byte)(mob.CurrentHP >> 8), (byte)(mob.CurrentHP),

                (byte)(mob.CurrentMP >> 24), (byte)(mob.CurrentMP >> 16),
                (byte)(mob.CurrentMP >> 8), (byte)(mob.CurrentMP),

                (byte)(mob.CurrentShield >> 24), (byte)(mob.CurrentShield >> 16),
                (byte)(mob.CurrentShield >> 8), (byte)(mob.CurrentShield)
            };
        }

        protected byte[] GetCharacterHP(int HP)
        {
            return new byte[NetWork.Server.CharacterHP.LENGTH]
            {
                NetWork.Server.CharacterHP.LENGTH >> 8,
                NetWork.Server.CharacterHP.LENGTH,

                (byte)(NetWork.Server.CharacterHP.TYPE >> 8),
                unchecked((byte)NetWork.Server.CharacterHP.TYPE),

                (byte)(HP >> 24), (byte)(HP >> 16),
                (byte)(HP >> 8), (byte)(HP),
            };
        }

        protected byte[] GetCharacterMP(int MP)
        {
            return new byte[NetWork.Server.CharacterMP.LENGTH]
            {
                NetWork.Server.CharacterMP.LENGTH >> 8,
                NetWork.Server.CharacterMP.LENGTH,

                (byte)(NetWork.Server.CharacterMP.TYPE >> 8),
                unchecked((byte)NetWork.Server.CharacterMP.TYPE),

                (byte)(MP >> 24), (byte)(MP >> 16),
                (byte)(MP >> 8), (byte)(MP),
            };
        }

        protected byte[] GetCharacterShield(int Shield)
        {
            return new byte[NetWork.Server.CharacterShield.LENGTH]
            {
                NetWork.Server.CharacterShield.LENGTH >> 8,
                NetWork.Server.CharacterShield.LENGTH,

                (byte)(NetWork.Server.CharacterShield.TYPE >> 8),
                unchecked((byte)NetWork.Server.CharacterShield.TYPE),

                (byte)(Shield >> 24), (byte)(Shield >> 16),
                (byte)(Shield >> 8), (byte)(Shield),
            };
        }

        protected byte[] GetCharacterCurrentHP(int HP)
        {
            return new byte[NetWork.Server.CharacterCurrentHP.LENGTH]
            {
                NetWork.Server.CharacterCurrentHP.LENGTH >> 8,
                NetWork.Server.CharacterCurrentHP.LENGTH,

                (byte)(NetWork.Server.CharacterCurrentHP.TYPE >> 8),
                unchecked((byte)NetWork.Server.CharacterCurrentHP.TYPE),

                (byte)(HP >> 24), (byte)(HP >> 16),
                (byte)(HP >> 8), (byte)(HP),
            };
        }

        protected byte[] GetCharacterCurrentMP(int MP)
        {
            return new byte[NetWork.Server.CharacterCurrentMP.LENGTH]
            {
                NetWork.Server.CharacterCurrentMP.LENGTH >> 8,
                NetWork.Server.CharacterCurrentMP.LENGTH,

                (byte)(NetWork.Server.CharacterCurrentMP.TYPE >> 8),
                unchecked((byte)NetWork.Server.CharacterCurrentMP.TYPE),

                (byte)(MP >> 24), (byte)(MP >> 16),
                (byte)(MP >> 8), (byte)(MP),
            };
        }

        protected byte[] GetCharacterCurrentShield(int Shield)
        {
            return new byte[NetWork.Server.CharacterCurrentShield.LENGTH]
            {
                NetWork.Server.CharacterCurrentShield.LENGTH >> 8,
                NetWork.Server.CharacterCurrentShield.LENGTH,

                (byte)(NetWork.Server.CharacterCurrentShield.TYPE >> 8),
                unchecked((byte)NetWork.Server.CharacterCurrentShield.TYPE),

                (byte)(Shield >> 24), (byte)(Shield >> 16),
                (byte)(Shield >> 8), (byte)(Shield),
            };
        }

        protected byte[] GetCreatingCharacterHMSBar()
        {
            int barName = Data.BarName;

            int HP = Data.HP; 
            int MP = Data.MP; 
            int Shield = Data.Shield;

            int currentHP = Data.CurrentHP; 
            int currentMP = Data.CurrentMP;
            int currentShield = Data.CurrentShield;

            return new byte[NetWork.Server.CreatingCharacterBar.LENGTH]
            {
                NetWork.Server.CreatingCharacterBar.LENGTH >> 8,
                NetWork.Server.CreatingCharacterBar.LENGTH,

                (byte)(NetWork.Server.CreatingCharacterBar.TYPE >> 8),
                unchecked((byte)NetWork.Server.CreatingCharacterBar.TYPE),

                (byte)(barName >> 8), (byte)(barName),

                (byte)(HP >> 24), (byte)(HP >> 16),
                (byte)(HP >> 8), (byte)(HP),

                (byte)(MP >> 24), (byte)(MP >> 16),
                (byte)(MP >> 8), (byte)(MP),

                (byte)(Shield >> 24), (byte)(Shield >> 16),
                (byte)(Shield >> 8), (byte)(Shield),

                (byte)(currentHP >> 24), (byte)(currentHP >> 16),
                (byte)(currentHP >> 8), (byte)(currentHP),

                (byte)(currentMP >> 24), (byte)(currentMP >> 16),
                (byte)(currentMP >> 8), (byte)(currentMP),

                (byte)(currentShield >> 24), (byte)(currentShield >> 16),
                (byte)(currentShield >> 8), (byte)(currentShield)
            };
        }

        /// <summary>
        /// Отправляет команду о начале обычной атаки.
        /// Скорость атаки передается из комнаты, так как она должна
        /// быть одинаковой от начала и до конца.
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        protected byte[] GetCharacterDefaultAttack(int speed, int damage)
        {
            return new byte[NetWork.Server.CharacterDefaultAttack.LENGTH]
            {
                NetWork.Server.CharacterDefaultAttack.LENGTH >> 8,
                NetWork.Server.CharacterDefaultAttack.LENGTH,

                (byte)(NetWork.Server.CharacterDefaultAttack.TYPE >> 8),
                (byte)NetWork.Server.CharacterDefaultAttack.TYPE,

                (byte)(speed >> 24), (byte)(speed >> 16), 
                (byte)(speed >> 8), (byte)(speed),

                (byte)(damage >> 24), (byte)(damage >> 16), 
                (byte)(damage >> 8), (byte)(damage)
            };
        }

        protected void LoggerInfo(string info)
        {
            if (StateInformation.IsCallConstruction)
                I_clientLogger.To(Logger.INFO, $"{NAME}:{GetKey()}[{info}]");
        }

        protected void LoggerError(string info)
        {
            if (StateInformation.IsCallConstruction)
                I_clientLogger.To(Logger.ERROR, $"{NAME}:{GetKey()}[{info}]");
        }

        protected void LoggerWarning(string info)
        {
            if (StateInformation.IsCallConstruction)
                I_clientLogger.To(Logger.WARNING, $"{NAME}:{GetKey()}[{info}]");
        }
    }
}