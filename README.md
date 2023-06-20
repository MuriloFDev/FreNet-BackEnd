# Frenet-BackEnd

API desenvolvida durante o processo seletivo da empresa Frenet é responsável por fornecer cotações de frete com base nos dados fornecidos pelo usuário. Ela possui uma classe chamada Quotation e um controlador chamado QuotationController.

No método QuoteShipping, a API recebe um objeto ShippingRequest que contém informações do cep origem e cep destino e os itens a serem enviados.
A API envia os dados para a API da Frenet, que realiza o cálculo do frete com base nessas informações. Em seguida, os dados de retorno são salvos na classe Quotation. A resposta enviada para o usuário é uma lista de objetos do tipo ReturnQuote, que contém detalhes sobre as cotações de frete.

O método GetQuoteByFreight recebe um CEP e a partir dele busca as 10 últimas cotações salvas no banco

![Quotation](https://github.com/MuriloFDev/Frenet-BackEnd/assets/137217345/c039daa2-f002-4975-84ba-a486caa2c0be)
