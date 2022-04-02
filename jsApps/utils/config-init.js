const fs = require("fs");

const baseDir = "./config";
const env = process.argv[2];

let data = JSON.parse(fs.readFileSync(`${baseDir}/config.json`));
data = { ...data, ...JSON.parse(fs.readFileSync(`${baseDir}/config.${env}.json`)) };

fs.writeFile("./public/config.json", JSON.stringify(data), (err) => {
    if (err) throw err;
    console.log(data);
});
