using Recommendation.Application.Common.Enams;
using Recommendation.Application.Common.Synchronizers.Interfaces;

namespace Recommendation.Application.Common.Synchronizers.Factory;

public class SynchronizationFactory : ISynchronizationFactory
{
    private readonly IEnumerable<ISynchronizer> _synchronizers;

    public SynchronizationFactory(IEnumerable<ISynchronizer> synchronizers)
    {
        _synchronizers = synchronizers;
    }

    public ISynchronizer GetInstance(TypeSync typeSync)
    {
        var instance = typeSync switch
        {
            TypeSync.LikeSynchronizer => GetService(typeof(LikeSynchronizer)),
            TypeSync.AverageRatingSynchronizer => GetService(typeof(AverageRatingSynchronizer)),
            TypeSync.EfAlgoliaSynchronizer => GetService(typeof(EfAlgoliaSynchronizer)),
            _ => throw new InvalidOperationException()
        };

        return instance;
    }
    
    private ISynchronizer GetService(Type type)
    {
        return _synchronizers.FirstOrDefault(x => x.GetType() == type)!;
    }
}