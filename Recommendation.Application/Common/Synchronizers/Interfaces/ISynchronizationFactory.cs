using Recommendation.Application.Common.Enams;

namespace Recommendation.Application.Common.Synchronizers.Interfaces;

public interface ISynchronizationFactory
{
    ISynchronizer GetInstance(TypeSync typeSync);
}