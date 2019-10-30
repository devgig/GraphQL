

using GraphQL.Types;
using NHLStats.Api.Helpers;
using NHLStats.Core.Data;
using NHLStats.Core.Models;

namespace NHLStats.Api.Models
{
    public class NHLStatsMutation : ObjectGraphType
    {
        public NHLStatsMutation(ContextServiceLocator contextServiceLocator)
        {
            Name = "PlayerMutation";

            Field<PlayerType>(
                "createPlayer",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<PlayerInputType>> { Name = "player" }
                ),
                resolve: context =>
                {
                    var player = context.GetArgument<Player>("player");
                    return contextServiceLocator.PlayerRepository.Add(player);
                });

            Field<PlayerType>(
             "updatePlayer",
             arguments: new QueryArguments(
                 new QueryArgument<NonNullGraphType<PlayerInputType>> { Name = "player" },
             ),
             resolve: context =>
             {
                 var player = context.GetArgument<Player>("player");
                 var item = contextServiceLocator.PlayerRepository.Update(player);
                 if (item == null)
                     context.Errors.Add(new GraphQL.ExecutionError($"Player: {player.Name} not found."));
                 return item;
             });

            Field<PlayerType>(
           "deletePlayer",
           arguments: new QueryArguments(
               new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }
           ),
           resolve: context =>
           {
               var id = context.GetArgument<int>("id");
               var result = contextServiceLocator.PlayerRepository.Delete(id);
               if (result.Result)
                   return $"The player with the id: {id} has been successfully deleted.";

               context.Errors.Add(new GraphQL.ExecutionError($"Player id: {id} not found."));
               return null;
           });
        }
    }
}
