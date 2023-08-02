$(document).ready(function() {
    // Find all article containers
    var $articles = $('.entry');

    // Find the height of the tallest article
    var maxHeight = Math.max.apply(
      null,
      $articles.map(function() {
        return $(this).outerHeight();
      })
    );

    // Set the height of all articles to the maxHeight
    $articles.css('min-height', maxHeight + 'px');
  });
