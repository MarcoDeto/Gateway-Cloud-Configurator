export const environment = {
  production: false,

  Error:             'https://localhost:5001/api/Error',
  Login:             'https://localhost:5001/api/User/Authenticate',
  Gateway:           'https://localhost:5001/api/Gateway',
  Firmware:          'https://localhost:5001/api/Firmware/Get?path=',
  InterfaceGroup:    'https://localhost:5001/api/GroupInterface',
  TypeEntity:        'https://localhost:5001/api/TypeEntity',
  Entity:            'https://localhost:5001/api/Entity',
  Interfaces:        'https://localhost:5001/api/Interfaces',
  ParamRegistry:     'https://localhost:5001/api/EntityParamRegistry',
  ParamValue:        'https://localhost:5001/api/EntityParamValue',
  Channels:          'https://localhost:5001/api/ChannelInterfaceValues',
  ChannelVariables:  'https://localhost:5001/api/ChannelInterfaceVariables',
  VirtualChannels:   'https://localhost:5001/api/VirtualChannelInterface',
  ChannelAssociate:  'https://localhost:5001/api/ChannelInterfaceAssociate',
};
