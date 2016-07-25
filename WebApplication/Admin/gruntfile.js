module.exports = function (grunt) {
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        sass: {
            dist: {
                options: {
                    style: 'compressed'
                },
                files: {
                    'Styles/admin.css': 'Styles/admin.scss'
                }
            }
        },
        postcss: {
            options: {
                processors: require('autoprefixer')({ browsers: 'last 2 versions' })
            },
            dist: {
                src: 'Styles/*.css'
            }
        },
        watch: {
            sass: {
                files: ['Styles/**/*.scss', 'Styles/scss/**/*.scss'],
                tasks: ['sass', 'postcss']
            }
        }
    });
    grunt.loadNpmTasks('grunt-contrib-sass');
    grunt.loadNpmTasks('grunt-postcss');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.registerTask('default', ['watch']);
}