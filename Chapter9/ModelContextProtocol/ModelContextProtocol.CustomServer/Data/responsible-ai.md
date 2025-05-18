---
title: Machine Learning Ethics with Responsible AI
lead: 6 Principles to help govern the ethical design and operation of machine learning systems
author: "Matt Eland"
tags: [Concepts,Ethics]
draft: false
date: "2021-11-08"
summary: "Ethics in AI is incredibly important. Let's take a look at Microsoft's 6 principles for responsible AI and explore each one in more depth."
thumbnail: ../../img/ResponsibleAIBanner.png
---

I recently discovered Microsoft's [responsible AI principles](https://www.microsoft.com/en-us/ai/responsible-ai) while studying for the [Azure AI Fundamentals certification]({{< ref "/post/ai900" >}}). I found it immensely reassuring that the first thing Microsoft stressed in my studies was the importance of responsible and ethical design of artificial intelligence systems.

Ethics in AI is incredibly important because it can be so easy for machine learning algorithms to accidentally pick up biases that were never desired or intended. What's worse is that the creators of those AI systems may not even realize that they've created a biased system because understanding AI models can be a very difficult thing to do.

*This content is also available in video form on [YouTube](https://www.youtube.com/watch?v=jp0MGRVtoWM)*
{{< youtube jp0MGRVtoWM >}}

## The Six Principles for Ethical AI Design

Let's take a look at Microsoft's 6 principles for responsible AI and discuss why they're so vitally important in the design, development, operation, and sales of AI systems regardless of if you're using cloud-based AI solutions or other options.

The 6 principles we'll be examining are:

- Fairness
- Reliability &amp; Safety
- Privacy &amp; Security
- Inclusiveness
- Transparency
- Accountability

### Fairness

The **fairness** principle involves treating all classes of users fairly. An AI should not discriminate against individual users based on ethnicity, gender, sexual orientation, religion, or any other factor.

**A critical note here is that fairness is not about the *intent* of the designers of the system, but instead about the actual behavior of the system**. One of the major drawbacks of AI is that it can be hard to determine *why* a system made a decision it did (see Transparency later) and so extreme care is needed to make sure that you don't feed an AI any data that may bias it against specific groups of individuals.

For example, if an AI system for a loan application took in fields including the applicant's postal code and the only people applying from a specific postal code are people with bad credit, a machine learning algorithm may learn to discriminate against individuals in that postal code. Due to historic racial segregation of neighborhoods, this could lead to a system overwhelmingly biased against certain groups of people.

Fairness requires that applications be given a diverse set of data of sufficient volume and carefully excluding any columns that might accidentally introduce bias into the system. Fairness also requires that AI solutions are monitored closely for continued fairness after deployment.

### Reliability &amp; Safety

**Reliability and safety** in AI systems involves making sure the algorithm can safely handle a wide range of scenarios that may come its way. 

Imagine you're a developer on a team working on self-driving cars. Your primary concern in that type of application would be ensuring the safety of the occupants of the vehicle and the other vehicles on the road. You'd need to test your AI in different lighting conditions, weather conditions, and many different areas. You'd need to test for situations where the roads were empty and when there were a large number of pedestrians, bicyclists, and other vehicles on the road.

Not everyone works on building self-driving cars (though that would be awesome!), but every AI system deployed needs a certain level of reliability and safety baked-in. If a system works fine in testing but can't keep up with sudden spikes in number of requests or is unable to reliably handle wildly different inputs in real world scenarios, it does not meet the reliability and safety standards required by an artificial intelligence solution.

### Privacy &amp; Security

**Privacy and security** involves ensuring that data that enters AI solutions is handled responsibly and securely.

In an ideal world, sensitive information would not need to travel across the internet. In fact, this *is* possible in some AI solutions with JavaScript and on-device machine learning frameworks available. Under these scenarios, sensitive data would be entered by a user, processed by a stored AI model operating in the browser or on the user's device, and then the sensitive data would be discarded without needing to be transmitted across the internet.

Additionally, if an AI doesn't need sensitive information such as social security numbers, names, phone numbers, E-Mail addresses, or other personally identifiable information (PII), it is best to exclude that information from anything sent to such an algorithm to reduce the *potential* for future violations.

However, sometimes sensitive data *is* needed and does need to be transmitted and potentially even stored in databases. The privacy and security principle involves ensuring all data that needs to be stored is stored with the appropriate level of respect and security.

### Inclusiveness

**Inclusiveness** states that artificial intelligence solutions should be for **all** people, regardless of any disabilities or impairments. Furthermore, Microsoft asserts that by designing our systems for those with the most limitations, we can extend those solutions to serve all other groups of people as well.

As someone who spent some time with paralysis a number of years ago - including the inability to smile for a period of time - I can relate strongly with this principle. For example, if an app developer used Azure's [cognitive services](https://accessibleai.dev/post/azure_cognitive_services) and its Face API to make sure users were smiling when creating their profile pictures or logging in, I wouldn't have been able to use the system.

Inclusiveness means designing your solutions with the hearing impaired, mute, color blind, and physically disabled in mind.

### Transparency

**Transparency** is an extremely interesting and compelling concern in AI systems. Many machine learning solutions - particularly neural nets - can come to a working solution for a problem that performs well, but it becomes very hard to understand *how* the algorithm arrived at the decisions it did.

The transparency principle has two components:

1. We need to be honest with our users about the potential limitations of our systems
2. We need to be able to understand how our systems are working and debug their operation

For the first principle, this means that we need to be transparent about the idea of potential inaccuracy inherent in machine learning solutions. For example, a machine learning solution that screens for an illness might need to disclose that false positives or false negatives are a potential.

A self-driving car algorithm might need to be transparent about the conditions the system is known to be fairly reliable in, but advocate for constant human monitoring and intervention or caution against certain weather conditions like fog or snow storms.

On the second point of transparency for debugging, a machine learning algorithm to review loan applications must offer transparency to your team so that the basis for its decision making process is evident in case it is discovered to be incorrect or biased. 

Thankfully, we are making leaps and bounds on adding explainability to our machine learning models - particularly in cloud-hosted environments like Microsoft Azure as is illustrated by the image below depicting some critical factors of evaluating a movie classification algorithm:

![Relevant components of movie classification](../../img/MovieFeatureImportance.png)

In the above image, the `release_month` column is the most important factor for classification with `Is Comedy` being shortly behind it in importance. We'll explore these diagrams and model explainability on Azure in future articles.

### Accountability

**Accountability** means that artificial intelligence solutions must be accountable to local, state, and federal authorities and relevant regulations as well as organizational leadership in the corporate world. 

Accountability also extends to the *creators* of AI solutions who must ensure that their applications conform to the other 5 principles.

Every system will have some risk for inaccuracy, but accountability is about minimizing all possible risk and identifying and eliminating any bias in our systems.

Accountability reinforces the message that ethics in AI cannot be an afterthought but instead must be a constant consideration throughout the design, development, evaluation, and operation of AI solutions.

## Conclusion

Regardless of what technologies you use and whether your AI solutions are on-device or in the cloud (or a combination of both), the ethical design and operation of artificial intelligence solutions is mandatory.

AI solutions are amazingly powerful and even beautiful things, but the possibility to accidentally allow bias into your systems **and not even notice it** needs to be under constant supervision during data acquisition, cleaning, feature selection and engineering, model evaluation, and ongoing operation of our machine learning solutions.

Personally, I find [Microsoft's 6 principles for responsible AI](https://www.microsoft.com/en-us/ai/responsible-ai) to be very compelling and a good first step for any team on the ethical design of artificial intelligence.

What do you think? What did Microsoft leave out? What other frameworks or principles do you look at when designing your systems?