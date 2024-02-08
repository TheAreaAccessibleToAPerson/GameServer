using System.Linq.Expressions;

namespace Items 
{
    public struct Resource : ISendItem
    {
        /// <summary>
        /// Имя предмета, имя кантинки, имя ассета итд.
        /// </summary>
        private readonly int _name;

        private int MAX_COUNT = 9999999;

        private readonly int _ID;
        private int _count;

        /// <summary>
        /// </summary>
        /// <param name="id">Для работы клиента с сервером.</param>
        /// <param name="count">Количесво</param>
        public Resource(int name, int id, int count)
        {
            _name = name;

            _ID = id;
            _count = count;
        }

        public void Append(int value)
        {
            _count += value;

            if (_count > MAX_COUNT) 
                _count = MAX_COUNT;
        }

        /// <summary>
        /// Вычетает указаную сумму из имеющейся.
        /// Если имеющиеся сумма больше вычетаемой return = true, isZero = false.
        /// Если имеющиеся сумма равна вычетаемой return = true, isZero = true, данный итем нужно удалить.
        /// Если имеющиеся сумма меньше вычетаемой return = false, isZero = false: ERROR!!!
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isZero"></param>
        /// <returns></returns>
        public bool TrySubsctract(int value, out bool isZero)
        {
            isZero = false;

            if (value < _count)
            {
                _count -= value;

                return true;
            }
            else 
            {
                if (value == _count)
                {
                    _count = 0;

                    isZero = true;
                }

                return false;
            }
        }

        public byte[] GetAddMessage()
        {
            return new byte[Message.Length.Resource.ADD] 
            {
                Message.Length.Resource.ADD >> 8,
                Message.Length.Resource.ADD,

                Type.RESOURCE >> 8, 
                Type.RESOURCE,

                // Тип сообщения.
                Message.ADD >> 8,
                Message.ADD,

                (byte)(_name >> 8),
                (byte)_name,
                
                (byte)(_ID >> 8),
                (byte)_ID,

                (byte)(_count >> 32),
                (byte)(_count >> 16),
                (byte)(_count >> 8),
                (byte)(_count)
            };
        }

        public byte[] GetDeleteMessage()
        {
            return new byte[Message.Length.Resource.DELETE] 
            {
                Message.Length.Resource.DELETE >> 8,
                Message.Length.Resource.DELETE,

                Type.RESOURCE >> 8, 
                Type.RESOURCE,

                // Тип сообщения.
                Message.DELETE >> 8,
                Message.DELETE,

                (byte)(_ID >> 8),
                (byte)_ID,
            };
        }

        public byte[] GetUpdateMessage()
        {
            return new byte[Message.Length.Resource.UPDATE] 
            {
                Message.Length.Resource.UPDATE >> 8,
                Message.Length.Resource.UPDATE,

                Type.RESOURCE >> 8, 
                Type.RESOURCE,

                // Тип сообщения.
                Message.UPDATE >> 8,
                Message.UPDATE,

                (byte)(_ID >> 8),
                (byte)_ID,

                (byte)(_count >> 32),
                (byte)(_count >> 16),
                (byte)(_count >> 8),
                (byte)(_count)
            };
        }

        public int GetName() => _name;
        public new int GetType() => Type.RESOURCE;
        public int GetID() => _ID;
    }
}