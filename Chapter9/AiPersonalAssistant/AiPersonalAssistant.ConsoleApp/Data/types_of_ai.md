---
title: Touring the Different Types of AI
lead: Exploring Content Generators, Application AI, Academic AI, and Game AI
summary: "Lets take a look at the various types of artificial intelligence out there: content generators, application AI, academic AI, and game AI."
canonical_url: https://newdevsguide.com/2023/02/25/touring-the-different-types-of-ai/
draft: false
date: "2023-02-25"
author: "Matt Eland"
thumbnail: ../img/A_TypesOfAI.png
tags: [Artificial Intelligence, GameDev, Concepts, Career]
showThumbnail: true
---

*This article was originally posted on [The New Dev's Guide](https://newdevsguide.com/2023/02/25/touring-the-different-types-of-ai/) on Feb 25th, 2023*

Artificial Intelligence is a fascinating and very broad field. In this article we’ll talk about the various types of AI and what you can do with each one.

Specifically we’ll cover content generation AI, application AI, academic forms of AI, and video game AI. My hope is that this article shows you the breadth of what’s currently possible under the broad umbrella of artificial intelligence.

## Content Generators

Let’s start with’ some of the newest forms of AI: the generative AI behind things like [ChatGPT](https://openai.com/blog/chatgpt/), [MidJourney](https://www.midjourney.com/), [DALL-E 2](https://openai.com/dall-e-2/), [GitHub Copilot](https://github.com/features/copilot), and other technologies.

These content generators use a machine learning technique built around something called Transformers to form large language models. Transformers allow the training algorithm to provide additional *context* about the order of words in sentences in a manner that can easily scale up to larger and larger models like the ones powering Chat GPT and GPT-3.

In a nutshell, these transformer-based architectures can find common patterns in the texts they’ve ‘been trained on and can respond to textual prompts with responses that look like they belong based on the patterns they’ve memorized in their training data.

As we’ve seen with ChatGPT and others, these systems are very powerful, but without regular retraining or more specialized code to augment the transformer-based model they will not learn anything new.”’

As I alluded to earlier with mentions of [MidJourney](https://www.midjourney.com/), [DALL-E 2](https://openai.com/dall-e-2/), and [GitHub Copilot](https://github.com/features/copilot), ‘these transformer-based models are not just for generating text. They also can be used to generate art, code, or other forms of expression.’

![An older man with colored spiky hair and a suit reviews some paper coming out of an antique machine in a workshop](../../img/ContentGenerators.png)

The artwork in this article was all generated using MidJourney.

For those interested in transformer-based models, I have a few articles you may want to investigate:

-   [How to Generate Text with OpenAI, GPT-3, and Python](https://accessibleai.dev/post/generating_text_with_gpt_and_python/)‘
-   [The Impact and Ethics of AI Art](https://accessibleai.dev/post/ethics_in_ai_art/)
-   [What Does ChatGPT Mean for New Software Developers?](https://accessibleai.dev/post/chatgpt_new_devs/)

## Application AI

![A young woman with colorful circuitry for air looks pensive](../../img/AppAI.png)

Application AI is a broad term I’m using here to describe a type of AI that adds AI capabilities to traditional applications.

These additional abilities typically come in a number of areas including:

-   **Speech Recognition** involves applications that recognize spoken or recording words and translate them to raw text.
-   **Speech Synthesis** takes strings of text and translates them to spoken words mimicking natural human speech
-   **Computer Vision** is a very broad area that involves detecting objects in an image, identifying faces, and generally allowing AI to make sense of visual data and the world around them.
-   **Text Analytics** involves analyzing text to identify languages, key phrases, known entities, and overall sentiment.
-   **Language Understanding** involves using natural language processing to map sentences from users to supported intents that the application knows how to handle.
-   **Decision making** some applications can use decision-trees and rules-based systems to respond to various scenarios
-   **Anomaly Detection** involves spotting anomalous or unusual behavior in streams of data. This is often used for fraud or virus detection.

As you can see, this application AI is a type of AI involving a wide range of capabilities. Each one of these capabilities has its own nuances and intricacies. Many experts pick a specific branch or set of branches within this to excel in, but there is certainly room for generalists that apply many of these capabilities to applications.

For those looking to experiment with application AI, I can think of no better way to get started than by looking into Cognitive Services on Microsoft Azure. [Azure Cognitive Services](https://azure.microsoft.com/en-us/products/cognitive-services/) is an API that you can interact with to add intelligent capabilities to your application regardless of what programming languages you use.

I’ve written [a number of articles around Azure Cognitive Services](https://accessibleai.dev/tags/cognitive-services/) including:

-   [Understanding Azure Cognitive Services](https://accessibleai.dev/post/cognitiveservices/)
-   [How to get started with Azure Cognitive Services](https://accessibleai.dev/post/azure_cognitive_services/)
-   [Computer Vision in C\# using Azure Cognitive Services](https://accessibleai.dev/post/computer-vision/)
-   [Key Phrase Extraction with Azure Cognitive Services in C\#](https://accessibleai.dev/post/key-phrase-extraction-with-azure-cognitive-services/)
-   [Text to Speech with Azure Cognitive Services in C\#](https://accessibleai.dev/post/text-to-speech-cognitive-services/)

## Academic AI

![A robot with the face of a young woman holds a book in a library](../../img/AcademicAI.png)

Academic AI is a branch of computer science focused on algorithms for optimally solving certain types of problems.

Academic AI involves a number of key branches including:

-   **Game Playing** – Finding the optimal strategy in games like chess and go by searching through different game states to increase odds of winning
-   **Pathfinding** – Finding the shortest paths between two or more locations
-   **Representing knowledge** and logical deduction capabilities to make associations, inferences, and solve logical puzzles
-   **Reinforcement Learning** allows AI systems to learn to achieve some task via positive and negative evaluation of their performance

While many applications of academic AI focus on playing games, these games are typically chosen as a means of **challenging the limits** of academic AI as opposed to presenting a compelling game playing experience.

Academic AI has its uses, and the solutions it has found in the pathfinding area in particular have grown outside of academia to empower games and other types of AI.

I have not written much about academic AI, but I recently published a fairly lengthy [introduction to reinforcement learning](https://accessibleai.dev/post/reinforcement_learning/) for those curious.

## Video Game AI

![A fierce looking woman stands in front of a chess board](../../img/GameAI.png)

Game AI is a type of AI that is focused on supporting a compelling and engaging game play experience by challenging the player and supporting the player’s immersion by acting in a believable way.

A shocking amount of game AI boils down to simply *not making stupid mistakes* that the player will notice.

Like other types of AI, video game AI has a number of different branches, mostly focusing on strategies for achieving their goals of creating an immersive and interesting play experience:

-   **Finite State Machines** represent simple AI agents with various states such as patrolling, searching, and attacking
-   **Hierarchical Finite State Machines** are finite state machines nested inside of larger finite state machines to help address the complexity of a large number of states
-   **Behavior trees** are simple but flexible structures that allow you to configure AI agents to do the most preferred available action following a simple, flexible, and understandable structure
-   **Utility Systems** use algorithms to determine the attractiveness of different actions an AI agent can take, then select the action with the highest score.
-   **Planning Systems** like GOAP and STRIPS perform backwards or forwards planning to figure out an optimal sequence of steps to move from the current world state to the desired state of the world

AI systems in games sometimes also focus on animation systems or deal with the overall play strategy by controlling waves of enemies or dynamically adjusting the challenge level. Many AI systems also govern the behavior of non-player character companions or bystanders in games.

All told, game AI is an interesting type of AI that focuses on creating compelling and convincing experiences instead of on finding a perfect solution to a problem.

## Closing Thoughts

We’ve now briefly toured the extent of content generation, application AI, academic AI, and game AI. Each of these branches is important and each one has their own distinct branches and focus areas.

My advice to you is to take a look at the things that interest you *most* in artificial intelligence and figure out which type of AI they belong to. Once you spot some trends there, explore other applications of AI within that same type and see what branches of AI within that type interest you the most.

AI shouldn’t be about the money or intrigue, but instead about the genuine interest in the applied use of computing power to tell a story, come to a convincing result, or to assist humanity as we build new things.

You might notice that I didn’t mention machine learning in this article. This is because I believe that machine learning is a subset of artificial intelligence and machine learning can play a role in any of these types of AI. If you’re curious about my thoughts on machine learning, I recommend you read my [AI vs Machine Learning](https://accessibleai.dev/post/aivsml/) article.

![Elements of Machine Learning](../../img/WhatIsAI2.png)

I’m curious what *your* favorite parts of AI are and why. If there’s an area you’re particularly interested in but haven’t explored yet, please let me know and I’ll see if it’s a good match for future content. Additionally, if you’ve fallen in love with AI and have stories to share, please let me know what you love.
