const { ethers } = require("hardhat");
require("dotenv").config({ path: ".env" });
const { WHITELIST_CONTRACT_ADDRESS, METADATA } = require("../constants");
async function main() {
  const whiteListContract = WHITELIST_CONTRACT_ADDRESS;
  const metaData = METADATA;

  const contract = await ethers.getContractFactory("MintNft");
  const deployedContract = await contract.deploy(metaData, whiteListContract);
  await deployedContract.deployed();

  console.log("Your deployed contract address is: ", deployedContract.address);
}

main()
  .then(() => process.exit(0))
  .catch((error) => {
    console.error(error);
    process.exit(1);
  });
