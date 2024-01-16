using System.ComponentModel;
using System.Net.NetworkInformation;

public struct NetWork 
{
    public const int HEADER_LENGTH = 4;

    public const int LENGTH_1BYTE_INDEX = 0;
    public const int LENGTH_2BYTE_INDEX = 1;

    public const int TYPE_1BYTE_INDEX = 2;
    public const int TYPE_2BYTE_INDEX = 3;

    public struct Server 
    {
        private const int ACCESS_VERIFICATION = 1;
        private const int CREATING_ROOM = 2;
        private const int NEXT_CHARACTER_POSITION = 3;
        private const int CREATING_CHARACTER = 4;
        private const int END_CREATING_SCANE = 5;


        /// <summary>
        /// Передать позицию для персонажа до которой нужно двигаться.
        /// </summary>
        private const int MOVE_CHARACTER_POSITION = 6;

        /// <summary>
        /// Начать бег персонажа.
        /// </summary>
        private const int CHARACTER_START_MOVE = 7;

        /// <summary>
        /// Остановить передвижение персонажa.
        /// </summary>
        private const int CHARACTER_STOP_MOVE = 8;

        /// <summary>
        /// Поворот персонажа в право.
        /// </summary>
        private const int CHARACTER_DIRECTION_RIGTH = 9;

        /// <summary>
        /// Поворот персонажа в лево.
        /// </summary>
        private const int CHARACTER_DIRECTION_LEFT = 10;

        /// <summary>
        /// Скорость бега.
        /// </summary>
        private const int CHARACTER_MOVE_SPEED = 11;

        /// <summary>
        /// Создать моба.
        /// </summary>
        private const int CREATING_MOB = 1000;

        /// <summary>
        /// Вражеский бар HP/MP/SHIELD
        /// </summary>
        private const int CREATING_ENEMY_MOB_BAR = 2000;

        /// <summary>
        /// Создает бар персонажа.
        /// </summary>
        private const int CREATING_CHARACTER_BAR = 2001;

        /// <summary>
        /// Передает общее количесво здоровья у персонажа.
        /// </summary>
        private const int CHARACTER_HP = 2002;

        /// <summary>
        /// Передает общее количесво маны у персонажа.
        /// </summary>
        private const int CHARACTER_MP = 2003;

        /// <summary>
        /// Передает общее количесво щита у персонажа.
        /// </summary>
        private const int CHARACTER_SHIELD = 2004;

        /// <summary>
        /// Передает общее количесво здоровья у вражеского моба.
        /// </summary>
        private const int ENEMY_MOB_HP = 2005;

        /// <summary>
        /// Передает общее количесво маны у вражеского моба.
        /// </summary>
        private const int ENEMY_MOB_MP = 2006;

        /// <summary>
        /// Передает общее количесво щата у вражеского моба.
        /// </summary>
        private const int ENEMY_MOB_SHIELD = 2007;


        /// <summary>
        /// Передает текущее количесво здоровья у персонажа.
        /// </summary>
        private const int CHARACTER_CURRENT_HP = 2008;

        /// <summary>
        /// Передает текущее количесво маны у персонажа.
        /// </summary>
        private const int CHARACTER_CURRENT_MP = 2009;

        /// <summary>
        /// Передает текущее количесво щита у персонажа.
        /// </summary>
        private const int CHARACTER_CURRENT_SHIELD = 2010;

        /// <summary>
        /// Передает текущее количесво hp у вражеского моба.
        /// </summary>
        private const int ENEMY_MOB_CURRENT_HP = 2011;
        /// <summary>
        /// Передает текущее количесво mp у вражеского моба.
        /// </summary>
        private const int ENEMY_MOB_CURRENT_MP = 2012;
        /// <summary>
        /// Передает текущее количесво shield у вражеского моба.
        /// </summary>
        private const int ENEMY_MOB_CURRENT_SHIELD = 2013;

        /// <summary>
        /// Ответ указывающий на успешную авторизацию.
        /// </summary>
        public struct AccessVerification
        {
            public const int TYPE = NetWork.Server.ACCESS_VERIFICATION;
            public const int LENGTH = NetWork.HEADER_LENGTH;
        }

        /// <summary>
        /// Создание комнаты и передача стартовой позиции.
        /// </summary>
        public struct CreatingRoom 
        {
            public const int TYPE = NetWork.Server.CREATING_ROOM;
            public const int LENGTH = NetWork.HEADER_LENGTH + 2;

            /// <summary>
            /// ID комнаты.(имя, оформление)
            /// </summary>
            public const int ROOM_ID_INDEX_1BYTE_INDEX = NetWork.HEADER_LENGTH;

            /// <summary>
            /// ID комнаты.(имя, оформление)
            /// </summary>
            public const int ROOM_ID_INDEX_2BYTE_INDEX = ROOM_ID_INDEX_1BYTE_INDEX + 1;
        }

        /// <summary>
        /// Новая позиция.
        /// </summary>
        public struct NextCharacterPosition 
        {
            public const int TYPE = NetWork.Server.NEXT_CHARACTER_POSITION;
            public const int LENGTH = NetWork.HEADER_LENGTH + 8;

            public const int POSITION_X_1BYTE_INDEX = NetWork.HEADER_LENGTH;
            public const int POSITION_X_2BYTE_INDEX = POSITION_X_1BYTE_INDEX + 1;
            public const int POSITION_X_3BYTE_INDEX = POSITION_X_2BYTE_INDEX + 1;
            public const int POSITION_X_4BYTE_INDEX = POSITION_X_3BYTE_INDEX + 1;

            public const int POSITION_Y_1BYTE_INDEX = POSITION_X_4BYTE_INDEX + 1;
            public const int POSITION_Y_2BYTE_INDEX = POSITION_Y_1BYTE_INDEX + 1;
            public const int POSITION_Y_3BYTE_INDEX = POSITION_Y_2BYTE_INDEX + 1;
            public const int POSITION_Y_4BYTE_INDEX = POSITION_Y_3BYTE_INDEX + 1;
        }

        public struct CreatingCharacter 
        {
            public const int TYPE = NetWork.Server.CREATING_CHARACTER;
            public const int LENGTH = NetWork.HEADER_LENGTH + 2;

            public const int CHARACTER_NAME_1BYTE_INDEX_INDEX = NetWork.HEADER_LENGTH;
            public const int CHARACTER_NAME_2BYTE_INDEX_INDEX = CHARACTER_NAME_1BYTE_INDEX_INDEX;
        }

        /// <summary>
        /// Конец формирования сцены.
        /// </summary>
        public struct EndCreatingScane
        {
            public const int TYPE = NetWork.Server.END_CREATING_SCANE;
            public const int LENGTH = NetWork.HEADER_LENGTH;
        }

        public struct CharacterMove 
        {
            public const int TYPE = NetWork.Server.MOVE_CHARACTER_POSITION;
            public const int LENGTH = NetWork.HEADER_LENGTH + 8;

            /// <summary>
            /// Позиция в которую нужно переместиться.
            /// </summary>
            public const int POSITION_X_1BYTE_INDEX = NetWork.HEADER_LENGTH;
            /// <summary>
            /// Позиция в которую нужно переместиться.
            /// </summary>
            public const int POSITION_X_2BYTE_INDEX = POSITION_X_1BYTE_INDEX + 1;

            /// <summary>
            /// Позиция в которую нужно переместиться.
            /// </summary>
            public const int POSITION_X_3BYTE_INDEX = POSITION_X_2BYTE_INDEX + 1;

            /// <summary>
            /// Позиция в которую нужно переместиться.
            /// </summary>
            public const int POSITION_X_4BYTE_INDEX = POSITION_X_3BYTE_INDEX + 1;

            /// <summary>
            /// Позиция в которую нужно переместиться.
            /// </summary>
            public const int POSITION_Y_1BYTE_INDEX = POSITION_X_4BYTE_INDEX + 1;

            /// <summary>
            /// Позиция в которую нужно переместиться.
            /// </summary>
            public const int POSITION_Y_2BYTE_INDEX = POSITION_Y_1BYTE_INDEX + 1;

            /// <summary>
            /// Позиция в которую нужно переместиться.
            /// </summary>
            public const int POSITION_Y_3BYTE_INDEX = POSITION_Y_2BYTE_INDEX + 1;

            /// <summary>
            /// Позиция в которую нужно переместиться.
            /// </summary>
            public const int POSITION_Y_4BYTE_INDEX = POSITION_Y_3BYTE_INDEX + 1;
        }

        public struct CharacterRun 
        {
            public const int TYPE = NetWork.Server.CHARACTER_START_MOVE;
            public const int LENGTH = NetWork.HEADER_LENGTH;
        }

        public struct CharacterStop
        {
            public const int TYPE = NetWork.Server.CHARACTER_STOP_MOVE;
            public const int LENGTH = NetWork.HEADER_LENGTH;
        }

        public struct CharacterDirectionRigth
        {
            public const int TYPE = NetWork.Server.CHARACTER_DIRECTION_RIGTH;
            public const int LENGTH = NetWork.HEADER_LENGTH;
        }

        public struct CharacterDirectionLeft
        {
            public const int TYPE = NetWork.Server.CHARACTER_DIRECTION_LEFT;
            public const int LENGTH = NetWork.HEADER_LENGTH;
        }

        public struct CharacterMoveSpeed
        {
            public const int TYPE = NetWork.Server.CHARACTER_MOVE_SPEED;
            public const int LENGTH = NetWork.HEADER_LENGTH + 2;

            public const int MOVE_SPEED_1BYTE_INDEX = NetWork.HEADER_LENGTH;
            public const int MOVE_SPEED_2BYTE_INDEX = MOVE_SPEED_1BYTE_INDEX + 1;
        }

        /// <summary>
        /// Создание моба.
        /// </summary>
        public struct CreatingMob 
        {
            public const int TYPE = NetWork.Server.CREATING_MOB;
            public const int LENGTH = NetWork.HEADER_LENGTH + 13;

            public const int MOB_NAME_1BYTE_INDEX = NetWork.HEADER_LENGTH;
            public const int MOB_NAME_2BYTE_INDEX = MOB_NAME_1BYTE_INDEX + 1;

            public const int MOB_ID_1BYTE_INDEX = MOB_NAME_2BYTE_INDEX + 1;
            public const int MOB_ID_2BYTE_INDEX = MOB_ID_1BYTE_INDEX + 1;

            public const int MOB_DIRECTION_INDEX = MOB_ID_2BYTE_INDEX + 1;

            public const int POSITION_X_1BYTE_INDEX = MOB_DIRECTION_INDEX + 1;
            public const int POSITION_X_2BYTE_INDEX = POSITION_X_1BYTE_INDEX + 1;
            public const int POSITION_X_3BYTE_INDEX = POSITION_X_2BYTE_INDEX + 1;
            public const int POSITION_X_4BYTE_INDEX = POSITION_X_3BYTE_INDEX + 1;

            public const int POSITION_Y_1BYTE_INDEX = POSITION_X_4BYTE_INDEX + 1;
            public const int POSITION_Y_2BYTE_INDEX = POSITION_Y_1BYTE_INDEX + 1;
            public const int POSITION_Y_3BYTE_INDEX = POSITION_Y_2BYTE_INDEX + 1;
            public const int POSITION_Y_4BYTE_INDEX = POSITION_Y_3BYTE_INDEX + 1;

        }

        public struct CreatingCharacterBar
        {
            public const int TYPE = NetWork.Server.CREATING_CHARACTER_BAR;
            public const int LENGTH = NetWork.HEADER_LENGTH + 26;

            public const int CREATING_BAR_NAME_1BYTE_INDEX = NetWork.HEADER_LENGTH;
            public const int CREATING_BAR_NAME_2BYTE_INDEX = CREATING_BAR_NAME_1BYTE_INDEX + 1;

            public const int CREATING_BAR_HP_1BYTE_INDEX = CREATING_BAR_NAME_2BYTE_INDEX + 1;
            public const int CREATING_BAR_HP_2BYTE_INDEX = CREATING_BAR_HP_1BYTE_INDEX + 1;
            public const int CREATING_BAR_HP_3BYTE_INDEX = CREATING_BAR_HP_2BYTE_INDEX + 1;
            public const int CREATING_BAR_HP_4BYTE_INDEX = CREATING_BAR_HP_3BYTE_INDEX + 1;

            public const int CREATING_BAR_MP_1BYTE_INDEX =  CREATING_BAR_HP_4BYTE_INDEX + 1;
            public const int CREATING_BAR_MP_2BYTE_INDEX =  CREATING_BAR_MP_1BYTE_INDEX + 1;
            public const int CREATING_BAR_MP_3BYTE_INDEX =  CREATING_BAR_MP_2BYTE_INDEX + 1;
            public const int CREATING_BAR_MP_4BYTE_INDEX =  CREATING_BAR_MP_3BYTE_INDEX + 1;

            public const int CREATING_BAR_SHIELD_1BYTE_INDEX = CREATING_BAR_MP_4BYTE_INDEX + 1;
            public const int CREATING_BAR_SHIELD_2BYTE_INDEX = CREATING_BAR_SHIELD_1BYTE_INDEX + 1;
            public const int CREATING_BAR_SHIELD_3BYTE_INDEX = CREATING_BAR_SHIELD_2BYTE_INDEX + 1;
            public const int CREATING_BAR_SHIELD_4BYTE_INDEX = CREATING_BAR_SHIELD_3BYTE_INDEX + 1;

            public const int CREATING_BAR_CURRENT_HP_1BYTE_INDEX = CREATING_BAR_SHIELD_4BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_HP_2BYTE_INDEX = CREATING_BAR_CURRENT_HP_1BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_HP_3BYTE_INDEX = CREATING_BAR_CURRENT_HP_2BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_HP_4BYTE_INDEX = CREATING_BAR_CURRENT_HP_3BYTE_INDEX + 1;

            public const int CREATING_BAR_CURRENT_MP_1BYTE_INDEX =  CREATING_BAR_CURRENT_HP_4BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_MP_2BYTE_INDEX =  CREATING_BAR_CURRENT_MP_1BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_MP_3BYTE_INDEX =  CREATING_BAR_CURRENT_MP_2BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_MP_4BYTE_INDEX =  CREATING_BAR_CURRENT_MP_3BYTE_INDEX + 1;

            public const int CREATING_BAR_CURRENT_SHIELD_1BYTE_INDEX = CREATING_BAR_CURRENT_MP_4BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_SHIELD_2BYTE_INDEX = CREATING_BAR_CURRENT_SHIELD_1BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_SHIELD_3BYTE_INDEX = CREATING_BAR_CURRENT_SHIELD_2BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_SHIELD_4BYTE_INDEX = CREATING_BAR_CURRENT_SHIELD_3BYTE_INDEX + 1;
        }

        public struct CreatingEnemyMobBar
        {
            public const int TYPE = NetWork.Server.CREATING_ENEMY_MOB_BAR;
            public const int LENGTH = NetWork.HEADER_LENGTH + 26;

            public const int CREATING_BAR_NAME_1BYTE_INDEX = NetWork.HEADER_LENGTH;
            public const int CREATING_BAR_NAME_2BYTE_INDEX = CREATING_BAR_NAME_1BYTE_INDEX + 1;

            public const int CREATING_BAR_HP_1BYTE_INDEX = CREATING_BAR_NAME_2BYTE_INDEX + 1;
            public const int CREATING_BAR_HP_2BYTE_INDEX = CREATING_BAR_HP_1BYTE_INDEX + 1;
            public const int CREATING_BAR_HP_3BYTE_INDEX = CREATING_BAR_HP_2BYTE_INDEX + 1;
            public const int CREATING_BAR_HP_4BYTE_INDEX = CREATING_BAR_HP_3BYTE_INDEX + 1;

            public const int CREATING_BAR_MP_1BYTE_INDEX =  CREATING_BAR_HP_4BYTE_INDEX + 1;
            public const int CREATING_BAR_MP_2BYTE_INDEX =  CREATING_BAR_MP_1BYTE_INDEX + 1;
            public const int CREATING_BAR_MP_3BYTE_INDEX =  CREATING_BAR_MP_2BYTE_INDEX + 1;
            public const int CREATING_BAR_MP_4BYTE_INDEX =  CREATING_BAR_MP_3BYTE_INDEX + 1;

            public const int CREATING_BAR_SHIELD_1BYTE_INDEX = CREATING_BAR_MP_4BYTE_INDEX + 1;
            public const int CREATING_BAR_SHIELD_2BYTE_INDEX = CREATING_BAR_SHIELD_1BYTE_INDEX + 1;
            public const int CREATING_BAR_SHIELD_3BYTE_INDEX = CREATING_BAR_SHIELD_2BYTE_INDEX + 1;
            public const int CREATING_BAR_SHIELD_4BYTE_INDEX = CREATING_BAR_SHIELD_3BYTE_INDEX + 1;

            public const int CREATING_BAR_CURRENT_HP_1BYTE_INDEX = CREATING_BAR_SHIELD_4BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_HP_2BYTE_INDEX = CREATING_BAR_CURRENT_HP_1BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_HP_3BYTE_INDEX = CREATING_BAR_CURRENT_HP_2BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_HP_4BYTE_INDEX = CREATING_BAR_CURRENT_HP_3BYTE_INDEX + 1;

            public const int CREATING_BAR_CURRENT_MP_1BYTE_INDEX =  CREATING_BAR_CURRENT_HP_4BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_MP_2BYTE_INDEX =  CREATING_BAR_CURRENT_MP_1BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_MP_3BYTE_INDEX =  CREATING_BAR_CURRENT_MP_2BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_MP_4BYTE_INDEX =  CREATING_BAR_CURRENT_MP_3BYTE_INDEX + 1;

            public const int CREATING_BAR_CURRENT_SHIELD_1BYTE_INDEX = CREATING_BAR_CURRENT_MP_4BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_SHIELD_2BYTE_INDEX = CREATING_BAR_CURRENT_SHIELD_1BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_SHIELD_3BYTE_INDEX = CREATING_BAR_CURRENT_SHIELD_2BYTE_INDEX + 1;
            public const int CREATING_BAR_CURRENT_SHIELD_4BYTE_INDEX = CREATING_BAR_CURRENT_SHIELD_3BYTE_INDEX + 1;
        }

        public struct CharacterHP 
        {
            public const int TYPE = NetWork.Server.CHARACTER_HP;
            public const int LENGTH = NetWork.HEADER_LENGTH + 4;

            public const int VALUE_1BYTE_INDEX = NetWork.HEADER_LENGTH + 1;
            public const int VALUE_2BYTE_INDEX = VALUE_1BYTE_INDEX + 1;
            public const int VALUE_3BYTE_INDEX = VALUE_2BYTE_INDEX + 1;
            public const int VALUE_4BYTE_INDEX = VALUE_3BYTE_INDEX + 1;
        }

        public struct CharacterMP
        {
            public const int TYPE = NetWork.Server.CHARACTER_MP;
            public const int LENGTH = NetWork.HEADER_LENGTH + 4;

            public const int VALUE_1BYTE_INDEX = NetWork.HEADER_LENGTH + 1;
            public const int VALUE_2BYTE_INDEX = VALUE_1BYTE_INDEX + 1;
            public const int VALUE_3BYTE_INDEX = VALUE_2BYTE_INDEX + 1;
            public const int VALUE_4BYTE_INDEX = VALUE_3BYTE_INDEX + 1;
        }

        public struct CharacterShield
        {
            public const int TYPE = NetWork.Server.CHARACTER_SHIELD;
            public const int LENGTH = NetWork.HEADER_LENGTH + 4;

            public const int VALUE_1BYTE_INDEX = NetWork.HEADER_LENGTH + 1;
            public const int VALUE_2BYTE_INDEX = VALUE_1BYTE_INDEX + 1;
            public const int VALUE_3BYTE_INDEX = VALUE_2BYTE_INDEX + 1;
            public const int VALUE_4BYTE_INDEX = VALUE_3BYTE_INDEX + 1;
        }

        public struct CharacterCurrentHP 
        {
            public const int TYPE = NetWork.Server.CHARACTER_CURRENT_HP;
            public const int LENGTH = NetWork.HEADER_LENGTH + 4;

            public const int VALUE_1BYTE_INDEX = NetWork.HEADER_LENGTH + 1;
            public const int VALUE_2BYTE_INDEX = VALUE_1BYTE_INDEX + 1;
            public const int VALUE_3BYTE_INDEX = VALUE_2BYTE_INDEX + 1;
            public const int VALUE_4BYTE_INDEX = VALUE_3BYTE_INDEX + 1;
        }

        public struct CharacterCurrentMP
        {
            public const int TYPE = NetWork.Server.CHARACTER_CURRENT_MP;
            public const int LENGTH = NetWork.HEADER_LENGTH + 4;

            public const int VALUE_1BYTE_INDEX = NetWork.HEADER_LENGTH + 1;
            public const int VALUE_2BYTE_INDEX = VALUE_1BYTE_INDEX + 1;
            public const int VALUE_3BYTE_INDEX = VALUE_2BYTE_INDEX + 1;
            public const int VALUE_4BYTE_INDEX = VALUE_3BYTE_INDEX + 1;
        }

        public struct CharacterCurrentShield
        {
            public const int TYPE = NetWork.Server.CHARACTER_CURRENT_SHIELD;
            public const int LENGTH = NetWork.HEADER_LENGTH + 4;

            public const int VALUE_1BYTE_INDEX = NetWork.HEADER_LENGTH + 1;
            public const int VALUE_2BYTE_INDEX = VALUE_1BYTE_INDEX + 1;
            public const int VALUE_3BYTE_INDEX = VALUE_2BYTE_INDEX + 1;
            public const int VALUE_4BYTE_INDEX = VALUE_3BYTE_INDEX + 1;
        }
    }

    public struct Client
    {
        private const int SENDING_LOGIN_AND_PASSWORD = 1;

        public struct SendingLoginAndPassword 
        {
            public const int TYPE = NetWork.Client.SENDING_LOGIN_AND_PASSWORD;
            public const int LENGTH = NetWork.HEADER_LENGTH + LOGIN_LENGTH + PASSWORD_LENGTH;

            public const int LOGIN_START_INDEX = NetWork.HEADER_LENGTH;
            public const int LOGIN_LENGTH = 16;
            public const int LOGIN_END_INDEX = LOGIN_START_INDEX + LOGIN_LENGTH;

            public const int PASSWORD_START_INDEX = LOGIN_START_INDEX + LOGIN_LENGTH;
            public const int PASSWORD_LENGTH = 16;
            public const int PASSWORD_END_INDEX = PASSWORD_START_INDEX + PASSWORD_LENGTH;
        }
    }
}