using CoreWebCommon.Dto;

namespace CoreLogic
{
    public class MasterLogic : _BaseLogic
    {
        public MasterLogic(Operation operation)
                : base(operation)
        {
        }

        public bool HasAuthority(int id, string key)
        {
            return true;
        }
    }
}