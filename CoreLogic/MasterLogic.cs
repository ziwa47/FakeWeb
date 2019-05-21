using CoreWebCommon.Dto;

namespace CoreLogic
{
    public class MasterLogic
    {
        public MasterLogic(Operation operation)
        {
        }

        public bool HasAuthority(int id, string key)
        {
            return true;
        }
    }
}