namespace NFTCollection.Contracts.MintNft

open System
open System.Threading.Tasks
open System.Collections.Generic
open System.Numerics
open Nethereum.Hex.HexTypes
open Nethereum.ABI.FunctionEncoding.Attributes
open Nethereum.Web3
open Nethereum.RPC.Eth.DTOs
open Nethereum.Contracts.CQS
open Nethereum.Contracts.ContractHandlers
open Nethereum.Contracts
open System.Threading
open NFTCollection.Contracts.MintNft.ContractDefinition


    type MintNftService (web3: Web3, contractAddress: string) =
    
        member val Web3 = web3 with get
        member val ContractHandler = web3.Eth.GetContractHandler(contractAddress) with get
    
        static member DeployContractAndWaitForReceiptAsync(web3: Web3, mintNftDeployment: MintNftDeployment, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> = 
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            web3.Eth.GetContractDeploymentHandler<MintNftDeployment>().SendRequestAndWaitForReceiptAsync(mintNftDeployment, cancellationTokenSourceVal)
        
        static member DeployContractAsync(web3: Web3, mintNftDeployment: MintNftDeployment): Task<string> =
            web3.Eth.GetContractDeploymentHandler<MintNftDeployment>().SendRequestAsync(mintNftDeployment)
        
        static member DeployContractAndGetServiceAsync(web3: Web3, mintNftDeployment: MintNftDeployment, ?cancellationTokenSource : CancellationTokenSource) = async {
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            let! receipt = MintNftService.DeployContractAndWaitForReceiptAsync(web3, mintNftDeployment, cancellationTokenSourceVal) |> Async.AwaitTask
            return new MintNftService(web3, receipt.ContractAddress);
            }
    
        member this.PriceQueryAsync(priceFunction: PriceFunction, ?blockParameter: BlockParameter): Task<BigInteger> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<PriceFunction, BigInteger>(priceFunction, blockParameterVal)
            
        member this.ApproveRequestAsync(approveFunction: ApproveFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(approveFunction);
        
        member this.ApproveRequestAndWaitForReceiptAsync(approveFunction: ApproveFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationTokenSourceVal);
        
        member this.BalanceOfQueryAsync(balanceOfFunction: BalanceOfFunction, ?blockParameter: BlockParameter): Task<BigInteger> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameterVal)
            
        member this.GetApprovedQueryAsync(getApprovedFunction: GetApprovedFunction, ?blockParameter: BlockParameter): Task<string> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<GetApprovedFunction, string>(getApprovedFunction, blockParameterVal)
            
        member this.IsApprovedForAllQueryAsync(isApprovedForAllFunction: IsApprovedForAllFunction, ?blockParameter: BlockParameter): Task<bool> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<IsApprovedForAllFunction, bool>(isApprovedForAllFunction, blockParameterVal)
            
        member this.MaxTokenIdsQueryAsync(maxTokenIdsFunction: MaxTokenIdsFunction, ?blockParameter: BlockParameter): Task<BigInteger> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<MaxTokenIdsFunction, BigInteger>(maxTokenIdsFunction, blockParameterVal)
            
        member this.NameQueryAsync(nameFunction: NameFunction, ?blockParameter: BlockParameter): Task<string> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<NameFunction, string>(nameFunction, blockParameterVal)
            
        member this.OwnerQueryAsync(ownerFunction: OwnerFunction, ?blockParameter: BlockParameter): Task<string> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameterVal)
            
        member this.OwnerOfQueryAsync(ownerOfFunction: OwnerOfFunction, ?blockParameter: BlockParameter): Task<string> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<OwnerOfFunction, string>(ownerOfFunction, blockParameterVal)
            
        member this.PresaleEndedQueryAsync(presaleEndedFunction: PresaleEndedFunction, ?blockParameter: BlockParameter): Task<BigInteger> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<PresaleEndedFunction, BigInteger>(presaleEndedFunction, blockParameterVal)
            
        member this.PresaleMintRequestAsync(presaleMintFunction: PresaleMintFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(presaleMintFunction);
        
        member this.PresaleMintRequestAndWaitForReceiptAsync(presaleMintFunction: PresaleMintFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(presaleMintFunction, cancellationTokenSourceVal);
        
        member this.PresaleStartedQueryAsync(presaleStartedFunction: PresaleStartedFunction, ?blockParameter: BlockParameter): Task<bool> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<PresaleStartedFunction, bool>(presaleStartedFunction, blockParameterVal)
            
        member this.RenounceOwnershipRequestAsync(renounceOwnershipFunction: RenounceOwnershipFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(renounceOwnershipFunction);
        
        member this.RenounceOwnershipRequestAndWaitForReceiptAsync(renounceOwnershipFunction: RenounceOwnershipFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(renounceOwnershipFunction, cancellationTokenSourceVal);
        
        member this.SafeTransferFromRequestAsync(safeTransferFromFunction: SafeTransferFromFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(safeTransferFromFunction);
        
        member this.SafeTransferFromRequestAndWaitForReceiptAsync(safeTransferFromFunction: SafeTransferFromFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(safeTransferFromFunction, cancellationTokenSourceVal);
        
        member this.SafeTransferFromRequestAsync(safeTransferFrom1Function: SafeTransferFrom1Function): Task<string> =
            this.ContractHandler.SendRequestAsync(safeTransferFrom1Function);
        
        member this.SafeTransferFromRequestAndWaitForReceiptAsync(safeTransferFrom1Function: SafeTransferFrom1Function, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(safeTransferFrom1Function, cancellationTokenSourceVal);
        
        member this.SetApprovalForAllRequestAsync(setApprovalForAllFunction: SetApprovalForAllFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(setApprovalForAllFunction);
        
        member this.SetApprovalForAllRequestAndWaitForReceiptAsync(setApprovalForAllFunction: SetApprovalForAllFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(setApprovalForAllFunction, cancellationTokenSourceVal);
        
        member this.StartPresaleRequestAsync(startPresaleFunction: StartPresaleFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(startPresaleFunction);
        
        member this.StartPresaleRequestAndWaitForReceiptAsync(startPresaleFunction: StartPresaleFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(startPresaleFunction, cancellationTokenSourceVal);
        
        member this.SupportsInterfaceQueryAsync(supportsInterfaceFunction: SupportsInterfaceFunction, ?blockParameter: BlockParameter): Task<bool> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<SupportsInterfaceFunction, bool>(supportsInterfaceFunction, blockParameterVal)
            
        member this.SymbolQueryAsync(symbolFunction: SymbolFunction, ?blockParameter: BlockParameter): Task<string> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<SymbolFunction, string>(symbolFunction, blockParameterVal)
            
        member this.TokenByIndexQueryAsync(tokenByIndexFunction: TokenByIndexFunction, ?blockParameter: BlockParameter): Task<BigInteger> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<TokenByIndexFunction, BigInteger>(tokenByIndexFunction, blockParameterVal)
            
        member this.TokenIdsQueryAsync(tokenIdsFunction: TokenIdsFunction, ?blockParameter: BlockParameter): Task<BigInteger> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<TokenIdsFunction, BigInteger>(tokenIdsFunction, blockParameterVal)
            
        member this.TokenOfOwnerByIndexQueryAsync(tokenOfOwnerByIndexFunction: TokenOfOwnerByIndexFunction, ?blockParameter: BlockParameter): Task<BigInteger> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<TokenOfOwnerByIndexFunction, BigInteger>(tokenOfOwnerByIndexFunction, blockParameterVal)
            
        member this.TokenURIQueryAsync(tokenURIFunction: TokenURIFunction, ?blockParameter: BlockParameter): Task<string> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<TokenURIFunction, string>(tokenURIFunction, blockParameterVal)
            
        member this.TotalSupplyQueryAsync(totalSupplyFunction: TotalSupplyFunction, ?blockParameter: BlockParameter): Task<BigInteger> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameterVal)
            
        member this.TransferFromRequestAsync(transferFromFunction: TransferFromFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(transferFromFunction);
        
        member this.TransferFromRequestAndWaitForReceiptAsync(transferFromFunction: TransferFromFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationTokenSourceVal);
        
        member this.TransferOwnershipRequestAsync(transferOwnershipFunction: TransferOwnershipFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(transferOwnershipFunction);
        
        member this.TransferOwnershipRequestAndWaitForReceiptAsync(transferOwnershipFunction: TransferOwnershipFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationTokenSourceVal);
        
    

