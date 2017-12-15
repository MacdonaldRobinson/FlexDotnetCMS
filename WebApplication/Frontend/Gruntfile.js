module.exports = function(grunt) {

  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),

    sass: {
      options: {
        sourceMap: true,
        style: 'compressed'
      },
      dist: {
        files: {
          'styles/css/main.css': 'styles/scss/main.scss'
        }
      }
    },

    postcss: {
      options: {
        map: true,
        use: [require('postcss-normalize')({})],
        use: [require('postcss-flexbugs-fixes')({})],
        processors: [
          require('autoprefixer')({
            browsers: ['last 2 versions', 'ie 9']
          }),
          require('cssnano')(),
        ]
      },
      dist: {
        src: 'styles/css/*.css'
      }
    },

    uglify: {
      dev: {
        options: {
          mangle: {
            reserved: ['jQuery']
          }
        },
        files: [{
          'scripts/js/main.min.js': ['scripts/js/main.js']
        }]
      }
    },

    sass_globbing: {
      includes: {
        files: {
          'styles/scss/_baseMap.scss': 'styles/scss/base/**/*.scss',
          'styles/scss/_componentsMap.scss': 'styles/scss/components/**/*.scss',
          'styles/scss/_layoutMap.scss': 'styles/scss/layout/**/*.scss',
          'styles/scss/_partialsMap.scss': 'styles/scss/partials/**/*.scss'
        },
        options: {
          useSingleQuotes: false
        }
      }
    },

    watch: {
      css: {
        files: ['styles/scss/**/*.{scss,sass}'],
        tasks: [
          'sass_globbing', 'sass', 'postcss'
        ],
        options: {
          spawn: false,
          livereload: false
        },
      }
    }

  });

  grunt.loadNpmTasks('grunt-sass');
  grunt.loadNpmTasks('grunt-postcss');
  grunt.loadNpmTasks('grunt-contrib-uglify');
  grunt.loadNpmTasks('grunt-contrib-watch');
  grunt.loadNpmTasks('grunt-sass-globbing');

  grunt.registerTask('default', ['watch']);

};
