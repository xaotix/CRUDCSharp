# CRUDCSharp
Simples CRUD com conceitos de padrões de projeto

Foi utilizada a criação de somente 1 tabela para exemplificar.

Exemplo de Singleton:
Função de chamada do banco offline
https://github.com/xaotix/CRUDCSharp/blob/master/CRUDCSharp/DBOffline.cs#L92

Exemplo de MonoState:
Get e set de propriedades das células contidas nas linhas
https://github.com/xaotix/CRUDCSharp/blob/master/CRUDCSharp/DBOffline.cs#L156

Exemplo de Log:
'Cria uma pasta no APPData e adiciona informações conforme as ações do banco'
https://github.com/xaotix/CRUDCSharp/blob/master/CRUDCSharp/DBOffline.cs#L35

As classes estão implementadas para serem serializadas em XML e salvas dentro de um arquivo ZIP:
Assim o banco fica salvo dentro de um arquivo quando a ferramenta é descarregada.
Funções Gravar e carregar da classe 'Tabela'
https://github.com/xaotix/CRUDCSharp/blob/master/CRUDCSharp/DBOffline.cs#L196
https://github.com/xaotix/CRUDCSharp/blob/master/CRUDCSharp/DBOffline.cs#L226


