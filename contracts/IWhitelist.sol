//SPDX-License-Identifier: UNLICENSED
pragma solidity >=0.8.4 <0.9.0;

//create an interface to whitelist-dapp
//https://github.com/sirval/whitelist-Dapp
interface IWhitelist {
    function whitelistedAddresses(address) external view returns (bool);
}
