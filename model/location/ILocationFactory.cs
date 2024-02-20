using Game_package;

namespace Location_package
{
    public interface ILocationFactory
    {
        public Location generate(int id);
    }
}