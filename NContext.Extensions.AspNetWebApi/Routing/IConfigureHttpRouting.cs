﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfigureHttpRouting.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2012 Waking Venture, Inc.
//
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//   and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//   of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//   TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//   DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Extensions.AspNetWebApi.Routing
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    /// Defines an HTTP routing configuration extensibility point for ASP.NET Web API. Allows 
    /// for routing configuration to be done outside the hosting application.
    /// </summary>
    [InheritedExport]
    public interface IConfigureHttpRouting
    {
        /// <summary>
        /// Gets the priority in which to configure the routing. Implementations will be run 
        /// in ascending order based on priority, so a lower priority value will execute first.
        /// </summary>
        /// <remarks></remarks>
        Int32 Priority { get; }

        /// <summary>
        /// Configures the specified HTTP routing manager.
        /// </summary>
        /// <param name="routingManager">The HTTP routing manager.</param>
        void Configure(IManageHttpRouting routingManager);
    }
}