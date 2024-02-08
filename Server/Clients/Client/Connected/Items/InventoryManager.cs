using System.Net.Http.Headers;
using Butterfly;

namespace server.client.items
{
    public struct InventoryManager
    {
        public InventoryManager(int size)
        {
            _size = size;

            _items = new object[size];
            _types = new int[size];
            _identifiers = new int[size];
        }

        private readonly int _size;

        private readonly object[] _items;
        private readonly int[] _types;
        private readonly int[] _identifiers;

        private int _count = 0;

        public bool TryAdd(int id, int type, object item, out string infoWarrning)
        {
            infoWarrning = null;

            if (_count < _size)
            {
                _items[_count] = item;
                _types[_count] = type;
                _identifiers[_count] = id;

                _count++;

                return true;
            }
            else 
            {
                infoWarrning = "Предмет не может быть добавлен в инвентарь, " +
                    $"так как привышен лимит в {_size} предметов.";

                return false;
            }
        }

        public bool TryGet(int id, out object item, out int type
            out string infoWarrning)
        {
            infoWarrning = null;

            for (int i = 0; i < _count; i++)
            {
                if (_identifiers[i] == id)
                {
                    type = _types[i];
                    item = _items[i];

                    return true;
                }
            }

            type = -1;
            item = null;

            infoWarrning = $"В инвенторе нету предмета с id:{id}";

            return false;
        }

        public bool TryDelete(int id, out object item, out int type)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_identifiers[i] == id)
                {
                    item = _items[i];
                    type = _types[i];

                    if (i == (_count - 1))
                    {
                        _items[i] = null;
                        _types[i] = -1;
                        _identifiers[i] = -1;
                    }
                    else
                    {
                        _items[i] = _items[_count - 1];
                        _types[i] = _types[_count - 1];
                        _identifiers[i] = _identifiers[_count - 1];

                        _items[_count - 1] = null;
                        _types[_count - 1] = -1;
                        _identifiers[_count - 1] = -1;
                    }

                    _count--;

                    return true;
                }
            }

            item = null;
            type = -1;

            return false;
        }
    }
}