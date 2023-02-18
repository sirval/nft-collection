export default function handler(req, res) {
  const tokenId = req.query.tokenId;

  console.error("error");
  const name = `Valoski NFT #${tokenId}`;
  const description =
    "Valoski NFT is my first detailed NFT in my course of learning web3 and Blockchain development";
  const image = `https://raw.githubusercontent.com/sirval/nft-collection/main/my-app/public/images/${
    Number(tokenId) - 1
  }.svg`;

  return res.status(200).json({
    name: name,
    description: description,
    image: image,
  });
}
