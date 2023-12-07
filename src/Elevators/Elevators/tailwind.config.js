/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./**/*.{html,razor,js,css}"],
  theme: {
  },
  plugins: [require("daisyui")],
  daisyui: {
    themes: [
      {
        mytheme: {

          "primary": "#5865f2",
          "secondary": "#eb459e",
          "accent": "#5865f2",
          "neutral": "#2c2f33",
          "base-100": "#2c2f33",
          "info": "#3abff8",
          "success": "#57f287",
          "warning": "#fee75c",
          "error": "#ed4245",
        },
      },
    ],
  },
}

