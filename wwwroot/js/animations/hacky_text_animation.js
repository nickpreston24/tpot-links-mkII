// const XRegExp = require("xregexp");
const letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

function hacky_text(event) {
    let interval = -1;
    let iteration = 0;
    clearInterval(interval);
    interval = setInterval(() => {
        event.target.innerText = event.target.innerText
            .split("")
            .map((letter, index) => {
                if (index < iteration) {
                    let ds = event.target.dataset.value;
                    return ds[index];
                }
                return letters[Math.floor(Math.random() * 26)]
            })
            .join("");
        if (iteration >= event.target.dataset.value.length) {
            clearInterval(interval);
        }
        iteration += 1 / 3;
    }, 30);
}

window.hacky_text = hacky_text;
//
// module.exports = {
//     hacky_text: hacky_text,
//     foo: ()=> {console.log('bar')}
// }