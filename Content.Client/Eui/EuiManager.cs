﻿using System;
using System.Collections.Generic;
using Content.Shared.Eui;
using Robust.Shared.IoC;
using Robust.Shared.Network;
using Robust.Shared.Reflection;

namespace Content.Client.Eui
{
    public sealed class EuiManager
    {
        [Dependency] private readonly IClientNetManager _net = default!;
        [Dependency] private readonly IReflectionManager _refl = default!;
        [Dependency] private readonly IDynamicTypeFactory _dtf = default!;

        private readonly Dictionary<uint, EuiData> _openUis = new();

        public void Initialize()
        {
            _net.RegisterNetMessage<MsgEuiCtl>(RxMsgCtl);
            _net.RegisterNetMessage<MsgEuiState>(RxMsgState);
            _net.RegisterNetMessage<MsgEuiMessage>(RxMsgMessage);
        }

        private void RxMsgMessage(MsgEuiMessage message)
        {
            var ui = _openUis[message.Id].Eui;
            ui.HandleMessage(message.Message);
        }

        private void RxMsgState(MsgEuiState message)
        {
            var ui = _openUis[message.Id].Eui;
            ui.HandleState(message.State);
        }

        private void RxMsgCtl(MsgEuiCtl message)
        {
            switch (message.Type)
            {
                case MsgEuiCtl.CtlType.Open:
                    var euiType = _refl.LooseGetType(message.OpenType);
                    var instance = _dtf.CreateInstance<BaseEui>(euiType);
                    instance.Initialize(this, message.Id);
                    instance.Opened();
                    if (_openUis.ContainsKey(message.Id))
                        message.Id++;
                    _openUis.Add(message.Id, new EuiData(instance));
                    break;

                case MsgEuiCtl.CtlType.Close:
                    var dat = _openUis[message.Id];
                    dat.Eui.Closed();
                    _openUis.Remove(message.Id);

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private sealed class EuiData
        {
            public readonly BaseEui Eui;

            public EuiData(BaseEui eui)
            {
                Eui = eui;
            }
        }
    }
}
