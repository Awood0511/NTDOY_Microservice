-----------API non-endpoint Requirements (possibly implemented with middleware)----------------

- Only accepts requests with valid Token (JWT Header = Authorization: Bearer [Token Here]) use middleware to auth and parse out user info into request headers

- One user may have multiple accounts (same token, different account passed into buy/sell transactions) specify account being used for buy and sell transactions

- Accesses tradier stock information for selected ticker symbol (middleware to get most recent tradier price)

- Logs all transactions in database (Logging middleware)

- start by buying 5k stocks to the main bank (admin user acount represents the bank)

- if a buy would put stock below a certain amount, buy enough to cover that sale plus a certain amount to cover future sales



-----------ENDPOINTS---------------
/api/quote : returns latest tradier stock quote for the ticker symbol
Inputs: Token in Auth Header
Outputs: json of all Tradier stock information

/api/transactions : returns all transaction logs to admin user (username = admin)
Inputs: Token in Auth Header
Outputs: json of all transactions from database table that stores logs

/api/sell : user sells a certain amount of stock in a specific account (must check if they have enough stocks to sell and give them the money)
Inputs: Token in Auth Header, account_name, buy_amount

/api/buy : user buys a certain amount of stock in a specific account (must check if they have the money and that OBS has enough stocks to sell)
Inputs: Token in Auth Header, account_name, sell_amount






