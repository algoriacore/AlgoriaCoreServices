﻿using AlgoriaCore.Application.QueriesAndCommands.Roles._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._2Queries
{
    public class RolGetByIdQuery : IRequest<RolResponse>
    {
        public long Id { get; set; }
    }
}
