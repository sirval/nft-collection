import Head from "next/head";
import Image from "next/image";
import styles from "@/styles/Home.module.css";
import { useEffect, useRef, useState } from "react";
import Web3Modal from "web3modal";
import { ethers, Contract, utils } from "ethers";
import swal from "sweetalert";
import { NFT_CONTRACT_ADDRESS, NFT_CONTRACT_ABI } from "@/constants";

export default function Home() {
  const [isOwner, setIsOwner] = useState(false);
  const [connectedWallet, setConnectedWallet] = useState(false);
  const [presaleStarted, setPresaleStarted] = useState(false);
  const [presaleEnded, setPresaleEnded] = useState(false);
  const [loading, setLoading] = useState(false);
  const [numTokenMinted, setNumTokenMinted] = useState("");

  const web3ModalRef = useRef();

  const getTotalTokenMinted = async () => {
    try {
      //get the provider
      const provider = await getProviderOrSigner();

      const contract = new Contract(
        NFT_CONTRACT_ADDRESS,
        NFT_CONTRACT_ABI,
        provider
      );

      const numTokenIds = await contract.tokenIds();
      setNumTokenMinted(numTokenIds.toString());
    } catch (error) {
      console.error(error);
    }
  };
  //presale mint
  const presaleMint = async () => {
    // setLoading(true);
    try {
      //get the signer as contract owner
      const signer = await getProviderOrSigner(true);

      const contract = new Contract(
        NFT_CONTRACT_ADDRESS,
        NFT_CONTRACT_ABI,
        signer
      );

      // start presale transaction
      const txn = await contract.presaleMint({
        value: utils.parseEther("0.01"),
      });
      setLoading(true);
      await txn.wait();
      swal({
        title: "success",
        icon: "success",
        text: "You successfully minted a CryptoDev",
      });
      setLoading(false);
    } catch (error) {
      console.error(error);
    }
    // setLoading(false);
  };

  //presale mint
  const publicMint = async () => {
    setLoading(true);
    try {
      //get the signer as contract owner
      const signer = await getProviderOrSigner(true);

      const contract = new Contract(
        NFT_CONTRACT_ADDRESS,
        NFT_CONTRACT_ABI,
        signer
      );

      // start presale transaction
      const txn = await contract.mint({
        value: utils.parseEther("0.01"),
      });
      await txn.wait();
      swal({
        title: "success",
        icon: "success",
        text: "You successfully minted a CryptoDev",
      });
    } catch (error) {
      console.error(error);
    }
    setLoading(false);
  };

  //check if the connected address is owner address
  const getOwner = async () => {
    setLoading(true);
    try {
      const provider = await getProviderOrSigner();

      const contract = new Contract(
        NFT_CONTRACT_ADDRESS,
        NFT_CONTRACT_ABI,
        provider
      );

      //get owner address
      const owner = await contract.owner();
      const signer = await getProviderOrSigner(true);
      const userAddress = await signer.getAddress();
      //compare both addresses for a match
      if (owner.toLowerCase() === userAddress.toLowerCase()) {
        setIsOwner(true);
      }
    } catch (error) {
      console.error(error);
    }
    setLoading(false);
  };

  //connect wallet
  const connectWallet = async () => {
    setLoading(true);
    try {
      await getProviderOrSigner();
      setConnectedWallet(true);
    } catch (error) {
      console.error(error);
    }
    setLoading(false);
  };

  //check if presale has started
  const checkIfPresaleStarted = async () => {
    try {
      //get the signer as contract owner
      const signer = await getProviderOrSigner(true);

      const contract = new Contract(
        NFT_CONTRACT_ADDRESS,
        NFT_CONTRACT_ABI,
        signer
      );

      const isPresaleStarted = await contract.presaleStarted();

      setPresaleStarted(isPresaleStarted);
    } catch (error) {
      console.error(error);
    }
  };

  //start presale
  const startPresale = async () => {
    setLoading(true);
    try {
      //get the signer as contract owner
      const signer = await getProviderOrSigner(true);

      const contract = new Contract(
        NFT_CONTRACT_ADDRESS,
        NFT_CONTRACT_ABI,
        signer
      );

      // start presale transaction
      const txn = await contract.startPresale();
      await txn.wait();
      setPresaleStarted(true);
    } catch (error) {
      console.error(error);
    }
    setLoading(false);
  };

  //check if presale has ended
  const checkIfPresaleEnded = async () => {
    try {
      //get the signer as contract owner
      const signer = await getProviderOrSigner(true);

      const contract = new Contract(
        NFT_CONTRACT_ADDRESS,
        NFT_CONTRACT_ABI,
        signer
      );

      const presaleEndTime = await contract.presaleEnded();
      const currentTimeInSeconds = Date.now() / 1000;
      const hasPresaleEnded = presaleEndTime.lt(
        Math.floor(currentTimeInSeconds)
      );

      setPresaleEnded(hasPresaleEnded);
    } catch (error) {
      console.error(error);
    }
  };

  //instantiate contract
  const nftContract = async () => {
    const contract = new Contract(
      NFT_CONTRACT_ADDRESS,
      NFT_CONTRACT_ABI,
      provider
    );

    //get presale state
    const isPresaleStarted = await nftContract.presaleStarted();
  };

  const getProviderOrSigner = async (needSigner = false) => {
    //pops metamask and request user to connect wallet
    const provider = await web3ModalRef.current.connect();
    const web3Provider = new ethers.providers.Web3Provider(provider);
    //if user is not connected to goerli tell them to connect
    const { chainId } = await web3Provider.getNetwork();
    if (chainId != 5) {
      swal({
        icon: "warning",
        text: "Switch to Goerli network",
      });

      throw new Error("Incorrect network");
    }

    //get signer
    if (needSigner) {
      const signer = web3Provider.getSigner();
      return signer;
    }
    return web3Provider;
  };

  //use async await function for presale state
  const onPageLoad = async () => {
    await connectWallet();
    await getOwner();
    const presaleStarted = await checkIfPresaleStarted();
    if (presaleStarted) {
      await checkIfPresaleEnded();
    }

    await getTotalTokenMinted();

    //get real time number of minted NFTs
    setInterval(async () => {
      await getTotalTokenMinted();
    }, 5000);

    //check if presale status on real time
    setInterval(async () => {
      const presaleStarted = await checkIfPresaleStarted();
      if (presaleStarted) {
        await checkIfPresaleEnded();
      }
    }, 5000);
  };

  useEffect(() => {
    if (!connectedWallet) {
      web3ModalRef.current = new Web3Modal({
        network: "goerli",
        providerOption: {},
        disableInjectedProvider: false,
      });
    }

    onPageLoad();
  }, []);

  function renderBody() {
    if (!connectedWallet) {
      return (
        <>
          <button className={styles.button} onClick={connectWallet}>
            Connect Wallect
          </button>
        </>
      );
    }

    if (loading) {
      return (
        <>
          <span className={styles.description}>Loading...</span>;
        </>
      );
    }
    if (isOwner && !presaleStarted) {
      //render button to start presale
      return (
        <>
          <button className={styles.button} onClick={startPresale}>
            Start Presale
          </button>
        </>
      );
    }

    if (!presaleStarted) {
      //notify to come back later
      return (
        <>
          <h3 style={{ color: "#ff7663" }}>
            Presale has not started yet. Please come back later!
          </h3>
        </>
      );
    }

    if (presaleStarted && !presaleEnded) {
      //allow to mint in presale
      //must join whitelist to be able to do this
      return (
        <div>
          <div className={styles.description}>
            Presale has started! If your address is whitelisted, you can mint a
            Valoski NFT{" "}
          </div>

          <button className={styles.button} onClick={presaleMint}>
            Presale Mint 🚀
          </button>
        </div>
      );
    }

    if (presaleEnded) {
      //allow users take part in public sale
      return (
        <div>
          <span className={styles.description}>
            Presale has ended. You can mint a CryptoDev in public sale, if any
            remain.
          </span>

          <button className={styles.button} onClick={publicMint}>
            Public Mint 🚀
          </button>
        </div>
      );
    }
  }
  return (
    <div>
      <Head>
        <title>Valoski NFT</title>
        <meta name="description" content="Whitelist-Dapp" />
        <link rel="icon" href="/favicon.ico" />
      </Head>
      <div className={styles.main}>
        <div>
          <h1 className={styles.title}>Welcome to Valoski NFT!</h1>
          <div className={styles.description}>
            Its an NFT collection made with{" "}
            <i style={{ color: "#FF7663", fontSize: "30px" }}> &#10084; </i>
          </div>
          <div className={styles.description}>
            {numTokenMinted}/20 have been minted
          </div>
          {renderBody()}
        </div>
        <div>
          <img className={styles.image} src="./images/0.svg" />
        </div>
      </div>

      <footer className={styles.footer}>
        Made with <i style={{ color: "#FF7663" }}> &#10084; </i> &nbsp;by &nbsp;
        <a href="https://ohukaiv.netlify.app/" target="_blank">
          Ikenna Valentine
        </a>
      </footer>
    </div>
  );
}
