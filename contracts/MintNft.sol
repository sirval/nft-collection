//SPDX-License-Identifier: UNLICENSED
pragma solidity >=0.8.4 <0.9.0;

import "@openzeppelin/contracts/token/ERC721/extensions/ERC721Enumerable.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "./IWhitelist.sol";

contract MintNft is ERC721Enumerable, Ownable {
    //define NFT baseUri
    string baseTokenUri;

    IWhitelist whitelist;
    bool public presaleStarted;
    uint256 public presaleEnded;
    uint256 public maxTokenIds = 20;
    uint256 public tokenIds;
    uint256 public _price = 0.01 ether;

    constructor(string memory baseUri, address whitelistContract)
        ERC721("Valoski", "OVI")
    {
        baseTokenUri = baseUri;
        whitelist = IWhitelist(whitelistContract);
    }

    //start a presale for the whitelisted addresses
    //use the onlyOwner modifier to make sure only the Owner starts the presale
    function startPresale() public onlyOwner {
        presaleStarted = true;
        presaleEnded = block.timestamp + 5 minutes;
    }

    //Allow those addresses that joined the presale to
    //to mint NFT for 5 minutes
    function presaleMint() public payable {
        //check if presale has started and still ongoing
        require(
            presaleStarted && block.timestamp <= presaleEnded,
            "Sorry, the presale has ended"
        );
        //check if address already joined the whitelist
        require(
            whitelist.whitelistedAddresses(msg.sender),
            "You didn't join the whitelist"
        );
        //check if tokenIds has exceeded the required number (20)
        require(tokenIds <= maxTokenIds, "Provided Limit Exceeded");
        //check if minting address sent required NFT price (0.01 ether)
        require(msg.value >= _price, "Ether sent is not correct");
        //increment the tokenId
        tokenIds += 1;
        _safeMint(msg.sender, tokenIds);
    }

    function mint() public payable {
        //check if presale has started and still ongoing
        require(
            presaleStarted && block.timestamp >= presaleEnded,
            "Sorry, the presale has not ended yet!"
        );
        //check if tokenIds has exceeded the required number (20)
        require(tokenIds <= maxTokenIds, "Provided Limit Exceeded");
        //check if minting address sent required NFT price (0.01 ether)
        require(msg.value >= _price, "Ether sent is not correct");
        //increment the tokenId
        tokenIds += 1;
        _safeMint(msg.sender, tokenIds);
    }

    receive() external payable {}

    fallback() external payable {}
}
