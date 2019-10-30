
using GraphQL.Types;
using NHLStats.Api.Helpers;
using NHLStats.Core.Data;


namespace NHLStats.Api.Models
{
    public class NHLStatsQuery : ObjectGraphType
    {
        public NHLStatsQuery(ContextServiceLocator contextServiceLocator)
        {
            Field<PlayerType>(
                "player",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context => contextServiceLocator.PlayerRepository.Get(context.GetArgument<int>("id")));

            Field<PlayerType>(
               "playerByWeight",
               arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "weight" }),
               resolve: context => contextServiceLocator.PlayerRepository.GetByWeight(context.GetArgument<int>("weight")));


            Field<PlayerType>(
                "randomPlayer",
                resolve: context => contextServiceLocator.PlayerRepository.GetRandom());

            Field<ListGraphType<PlayerType>>(
                "players",
                resolve: context => contextServiceLocator.PlayerRepository.All());
        }
    }
}


 