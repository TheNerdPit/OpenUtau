﻿using DynamicData.Binding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OpenUtau.Core.DawIntegration;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUtau.App.ViewModels {
    public class DawIntegrationTerminalViewModel : ViewModelBase {
        [Reactive] public DawIntegrationServer? SelectedServer { get; set; } = null;
        [Reactive]
        public ObservableCollectionExtended<DawIntegrationServer> DawIntegrationServers { get; set; }

        public DawIntegrationTerminalViewModel() {
            DawIntegrationServers = new ObservableCollectionExtended<DawIntegrationServer>();
            Task.Run(async () => {
                var servers = await ServerFinder.FindServers();

                var serverList = servers.Select((server) => new DawIntegrationServer(server)).ToList();
                if (servers.Count == 0) {
                    DawIntegrationServers.Add(new DawIntegrationServer(null));
                    SelectedServer = null;
                } else {
                    DawIntegrationServers.Load(serverList);
                    SelectedServer = DawIntegrationServers[0];
                }
            });

        }
    }

    public class DawIntegrationServer {
        public Server? server;
        public bool Enabled {
            get {
                return server != null;
            }
        }
        public DawIntegrationServer(Server? server) {
            this.server = server;
        }

        public override string ToString() {
            if (server != null) {
                return server.Name;
            } else {
                return "{DynamicResource dawintegrationterminal.noservers}";
            }
        }
    }


}
