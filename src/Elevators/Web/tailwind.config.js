/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./**/*.{html,razor,js}"],
  theme: {
    extend: {},
  },
  plugins: [require("daisyui")],
  daisyui:{
    themes: ["light"]
  }
}

