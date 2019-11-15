using GraphQL.Types;
using NHLStats.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHLStats.Api.Models
{
    public class PlayerLevelEnumType : EnumerationGraphType<PlayerLevel>
    {
        public PlayerLevelEnumType()
        {
            Name = "Level";
            Description = "The players professional level";

        }
    }

   
}
