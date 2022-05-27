using Games.Domain;

namespace Games.Data
{
    public interface IRemoveChecerRepository
    {
        Checker GetRemoveChecker(int row, int column);

    }
}
