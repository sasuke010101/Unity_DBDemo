using System.Collections.Generic;

namespace DragonBones
{
    abstract public class BaseObject
    {
        private static uint _defaultMaxCount = 5000;
        private static readonly Dictionary<System.Type, uint> _maxCountMap = new Dictionary<System.Type, uint>();
        private static readonly Dictionary<System.Type, List<BaseObject>> _poolsMap = new Dictionary<System.Type, List<BaseObject>>();
        
        private static void _returnObject(BaseObject obj)
        {
            var classType = obj.GetType();
            var maxCount = _maxCountMap.ContainsKey(classType) ? _maxCountMap[classType] : _defaultMaxCount;
            var pool = _poolsMap.ContainsKey(classType) ? _poolsMap[classType] : _poolsMap[classType] = new List<BaseObject>();

            if (pool.Count < maxCount)
            {
                if (!pool.Contains(obj))
                {
                    pool.Add(obj);
                }
                else
                {
                    DragonBones.Warn("");
                }
            }
        }

        /**
         * @language zh_CN
         * ����ÿ�ֶ���ص���󻺴�������
         * @param classType �������͡�
         * @param maxCount ��󻺴������� (����Ϊ 0 �򲻻���)
         * @version DragonBones 4.5
         */
        public static void SetMaxCount(System.Type classType, uint maxCount)
        {
            if (classType != null)
            {
                _maxCountMap[classType] = maxCount;
                if (_poolsMap.ContainsKey(classType))
                {
                    var pool = _poolsMap[classType];
                    if (pool.Count > maxCount)
                    {
                        //pool.Count = maxCount;
                    }
                }
            }
            else
            {
                _defaultMaxCount = maxCount;
                foreach (var pair in _poolsMap)
                {
                    if (!_maxCountMap.ContainsKey(pair.Key))
                    {
                        continue;
                    }

                    _maxCountMap[pair.Key] = maxCount;

                    var pool = _poolsMap[pair.Key];
                    if (pool.Count > maxCount)
                    {
                        //pool.Count = maxCount;
                    }
                }
            }
        }

        /**
         * @language zh_CN
         * �������ػ���Ķ���
         * @param objectConstructor �������͡� (��������������л���)
         * @version DragonBones 4.5
         */
        public static void ClearPool(System.Type classType)
        {
            if (classType != null)
            {
                if (_poolsMap.ContainsKey(classType))
                {
                    var pool = _poolsMap[classType];
                    if (pool.Count > 0)
                    {
                        pool.Clear();
                    }
                }
            }
            else
            {
                foreach (var pair in _poolsMap)
                {
                    var pool = _poolsMap[pair.Key];
                    if (pool.Count > 0)
                    {
                        pool.Clear();
                    }
                }
            }
        }

        /**
         * @language zh_CN
         * �Ӷ�����д���ָ������
         * @param objectConstructor �����ࡣ
         * @version DragonBones 4.5
         */
        public static T BorrowObject<T>() where T : BaseObject, new()
        {
            var type = typeof(T);
            var pool = _poolsMap.ContainsKey(type) ? _poolsMap[type] : null;
            if (pool != null && pool.Count > 0)
            {
                var index = pool.Count - 1;
                var obj = pool[index];
                pool.RemoveAt(index);
                return (T)obj;
            }
            else
            {
                var obj = new T();
                obj._onClear();
                return obj;
            }
        }

        protected BaseObject()
        {
        }

        /**
         * @private
         */
        abstract protected void _onClear();

        /**
         * @language zh_CN
         * ������ݲ���������ء�
         * @version DragonBones 4.5
         */
        public void ReturnToPool()
        {
            _onClear();
            _returnObject(this);
        }
    }
}

