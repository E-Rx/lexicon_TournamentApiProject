﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dtos
{
    public class TournamentDetailsPagedDto
    {
        public List<TournamentDetailsDto>? Items { get; set; }
        public int TotalCount { get; set; }
    }
}
