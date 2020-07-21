module.exports = {
    root: true,
    parserOptions: {
      parser: 'babel-eslint'
    },
    env: {
      node: true
    },
    extends: [
      'plugin:vue/recommended'
    ],
    plugins: [
          'vue'
      ],
    rules: {
        'no-console': 'off',
        // allow async-await
        'generator-star-spacing': 'off',
        // allow debugger during development
        'no-debugger': process.env.NODE_ENV === 'production' ? 'error' : 'off',
        'no-tabs': 'off', // http://eslint.org/docs/rules/no-tabs
        'no-var': 'error', // http://eslint.org/docs/rules/no-var
        'indent': [2, 'tab', { 'SwitchCase': 1 }], // http://eslint.org/docs/rules/indent
        'semi': ['error', 'always'], // http://eslint.org/docs/rules/semi
        'max-len': 'off',
        'space-before-function-paren': 'off', // http://eslint.org/docs/rules/space-before-function-paren
        'vue/max-attributes-per-line': 'off', // https://github.com/vuejs/eslint-plugin-vue/blob/master/docs/rules/max-attributes-per-line.md
        'vue/html-indent': ['error', 'tab'], // https://github.com/vuejs/eslint-plugin-vue/blob/master/docs/rules/html-indent.md
        'quotes': ["error", "single", { "avoidEscape": true }] // https://eslint.org/docs/rules/quotes
    }
  };
  