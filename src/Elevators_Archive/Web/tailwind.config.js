/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./**/*.{html,razor,js}"],
  theme: {
    extend: {
      colors:{
        "primary": "hsl(var(--p) / <alpha-value>)",
        "secondary": "hsl(var(--s) / <alpha-value>)",
        "accent": "hsl(var(--a) / <alpha-value>)",
        "neutral": "hsl(var(--n) / <alpha-value>)",
        "base-100": "hsl(var(--b1) / <alpha-value>)",
        "info": "hsl(var(--in) / <alpha-value>)",
        "success": "hsl(var(--su) / <alpha-value>)",
        "warning": "hsl(var(--wa) / <alpha-value>)",
        "error": "hsl(var(--er) / <alpha-value>)",
      },
    },
  },
  plugins: [require("daisyui")],
  daisyui:{
    themes:
    [
      {
        mytheme: {
          "primary": "#5865F2",
          "secondary": "#EB459E",
          "accent": "#5865f2",
          "neutral": "#2c2f33",
          "base-100": "#2c2f33",
          "info": "#3abff8",
          "success": "#57F287",
          "warning": "#FEE75C",
          "error": "#ED4245",
        },
      },
    ],
  }
}

