# CRUDCSharp
Simples CRUD com conceitos de padrões de projeto


Exemplo de Singleton:
Função de chamada do banco offline
*public static DBOffline GetDBOffline()*
https://github.com/xaotix/CRUDCSharp/blob/master/CRUDCSharp/DBOffline.cs#L92

Exemplo de MonoState:
Get e set de propriedades das células contidas nas linhas
https://github.com/xaotix/CRUDCSharp/blob/master/CRUDCSharp/DBOffline.cs#L156
*public Celula get(string Coluna)*


Exemplo de Log:
'Cria uma pasta no APPData e adiciona informações conforme as ações do banco'
https://github.com/xaotix/CRUDCSharp/blob/master/CRUDCSharp/DBOffline.cs#L35


As classes estão implementadas para serem serializadas em XML e salvas dentro de um arquivo ZIP:
Funções Gravar e carregar da classe 'Tabela'
https://github.com/xaotix/CRUDCSharp/blob/master/CRUDCSharp/DBOffline.cs#L196
https://github.com/xaotix/CRUDCSharp/blob/master/CRUDCSharp/DBOffline.cs#L226


