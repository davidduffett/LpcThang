# Lpc Thang

ASP.NET MVC Helper methods for implementing LivePerson Chat.

## Introduction

Implementing LivePerson Chat (Lpc) in an ASP.NET MVC website involves the following steps:
1. Initialise a global JavaScript variable `lpUnit` to the business unit that corresponds with the chat routing you have setup in the LPC Admin Console.
2. Include a script reference to `mtagconfig.js` which will be provided to you by LivePerson.
3. Include a script containing custom variables, including such things as the section of the site, customer or order details.
4. Optionally include a script for ecommerce conversions.

The aim of Lpc Thang is to simplify the process of including these scripts in your MVC application by providing easy to use Helper methods and action filter attributes.
