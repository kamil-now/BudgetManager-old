module.exports = {
  root: true,
  env: {
    "vue/setup-compiler-macros": true,
    "node": true,
  },
  extends: [
    "plugin:vue/vue3-essential",
    "@vue/typescript/recommended",
    "eslint:recommended",
  ],
  parserOptions: {
    ecmaVersion: 2020,
    parser: "@typescript-eslint/parser",
  },
  rules: {
    "no-console": process.env.NODE_ENV === "production" ? "warn" : "off",
    "no-debugger": process.env.NODE_ENV === "production" ? "warn" : "off",
    "linebreak-style": "off",
    "@typescript-eslint/no-unused-vars": [
      "warn",
      {
        argsIgnorePattern: "^_+$",
        varsIgnorePattern: "^_+$",
      },
    ],
    "newline-per-chained-call": ["warn", { ignoreChainWithDepth: 1 }],
    "no-multiple-empty-lines": "warn",
    "padding-line-between-statements": [
      "warn",
      { blankLine: "never", prev: "import", next: "import" },
    ],
    "quotes": ["warn", "double"],
    "func-call-spacing": ["warn", "never"],
    "no-empty-pattern": "off",
    "no-prototype-builtins": "off",
    "no-new-func": "warn",
    "no-redeclare": "warn",
    "no-return-await": "warn",
    "no-sparse-arrays": "warn",
    "no-void": "warn",
    "quote-props": ["warn", "consistent-as-needed"],
    "space-before-blocks": "warn",
    "keyword-spacing": [
      "warn",
      {
        before: true,
        after: true
      }
    ],
    "space-infix-ops": "warn",
    "space-unary-ops": "warn",
    "object-curly-spacing": ["warn", "always"],
    "comma-spacing": ["warn", { before: false, after: true }],
    "array-bracket-spacing": ["warn", "never"],
    "arrow-spacing": ["warn", { before: true, after: true }],
    "key-spacing": [2, { beforeColon: false, afterColon: true }],
    "semi": ["warn", "always"],
    "indent": ["warn", 2]
  },
  overrides: [
    {
      files: ["**/__tests__/*.{j,t}s?(x)", "**/tests/**/*.spec.{j,t}s?(x)"],
      env: {
        jest: true,
      },
      rules: {
        "@typescript-eslint/no-shadow": [
          "warn",
          { ignoreTypeValueShadow: true },
        ],
      },
    },
  ],
};
